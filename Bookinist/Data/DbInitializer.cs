using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookinist.Data;

internal class DbInitializer
{
    private readonly BookinistDb _db;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(BookinistDb db, ILogger<DbInitializer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Инициализация БД...");

        //_logger.LogInformation("Удаление существующей БД...");
        //await _db.Database.EnsureDeletedAsync().ConfigureAwait(false);
        //_logger.LogInformation($"Удаление существующей БД выполнено за {timer.ElapsedMilliseconds} мс");

        _logger.LogInformation("Миграция БД...");
        await _db.Database.MigrateAsync().ConfigureAwait(false);
        _logger.LogInformation($"Миграция БД выполнена за {timer.ElapsedMilliseconds} мс");

        if (await _db.Books.AnyAsync()) return;

        await InitializeCategoriesAsync();
        await InitializeBooksAsync();
        await InitializeSellersAsync();
        await InitializeBuyersAsync();
        await InitializeDealsAsync();

        _logger.LogInformation($"Инициализация БД выполнена за {timer.Elapsed.TotalSeconds} с");
    }

    #region Categories

    private const int CategoriesCount = 10;

    private Category[] _categories;

    private async Task InitializeCategoriesAsync()
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Инициализация категорий...");

        _categories = new Category[CategoriesCount];
        for (var i = 0; i < CategoriesCount; i++)
            _categories[i] = new Category { Name = $"Категория {i + 1}" };

        await _db.Categories.AddRangeAsync(_categories);
        await _db.SaveChangesAsync();

        _logger.LogInformation($"Инициализация категорий выполнена за {timer.ElapsedMilliseconds} мс");
    }

    #endregion

    #region Books

    private const int BooksCount = 10;

    private Book[] _books;

    private async Task InitializeBooksAsync()
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Инициализация книг...");

        var random = new Random(DateTime.Now.Millisecond);
        _books = Enumerable.Range(1, BooksCount)
            .Select(b => new Book
            {
                Name = $"Книга {b}",
                Category = random.NextItem(_categories)
            })
            .ToArray();

        await _db.Books.AddRangeAsync(_books);
        await _db.SaveChangesAsync();

        _logger.LogInformation($"Инициализация книг выполнена за {timer.ElapsedMilliseconds} мс");
    }

    #endregion

    #region Seller

    private const int SellersCount = 10;

    private Seller[] _sellers;

    private async Task InitializeSellersAsync()
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Инициализация продавцов...");

        _sellers = Enumerable.Range(1, SellersCount)
            .Select(s => new Seller
            {
                Name = $"Продавец-Имя {s}",
                Surname = $"Продавец-Фамилия {s}",
                Patronymic = $"Продавец-Отчество {s}"
            })
            .ToArray();

        await _db.Sellers.AddRangeAsync(_sellers);
        await _db.SaveChangesAsync();

        _logger.LogInformation($"Инициализация продавцов выполнена за {timer.ElapsedMilliseconds} мс");
    }

    #endregion

    #region Buyer

    private const int BuyersCount = 10;

    private Buyer[] _buyers;

    private async Task InitializeBuyersAsync()
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Инициализация покупателей...");

        _buyers = Enumerable.Range(1, BuyersCount)
            .Select(s => new Buyer
            {
                Name = $"Покупатель-Имя {s}",
                Surname = $"Покупатель-Фамилия {s}",
                Patronymic = $"Покупатель-Отчество {s}"
            })
            .ToArray();

        await _db.Buyers.AddRangeAsync(_buyers);
        await _db.SaveChangesAsync();

        _logger.LogInformation($"Инициализация покупателей выполнена за {timer.ElapsedMilliseconds} мс");
    }

    #endregion

    #region Deal

    private const int DealsCount = 1000;

    private async Task InitializeDealsAsync()
    {
        var timer = Stopwatch.StartNew();
        _logger.LogInformation("Инициализация сделок...");

        var random = new Random(DateTime.Now.Millisecond);

        var deals = Enumerable.Range(1, DealsCount)
            .Select(d => new Deal
            {
                Book = random.NextItem(_books),
                Seller = random.NextItem(_sellers),
                Buyer = random.NextItem(_buyers),
                Price = (decimal) (random.NextDouble() * 4000 + 700)
            });

        await _db.Deals.AddRangeAsync(deals);
        await _db.SaveChangesAsync();

        _logger.LogInformation($"Инициализация сделок выполнена за {timer.ElapsedMilliseconds} мс");
    }

    #endregion
}