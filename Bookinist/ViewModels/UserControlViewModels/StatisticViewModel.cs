using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Models;
using Bookinist.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookinist.ViewModels.UserControlViewModels;

internal class StatisticViewModel : ViewModel
{
    #region Fields

    private readonly IRepository<Book> _books;
    private readonly IRepository<Buyer> _buyers;
    private readonly IRepository<Seller> _sellers;
    private readonly IRepository<Deal> _deals;

    public ObservableCollection<BestsellerInfo> Bestsellers { get; } = new();

    #endregion

    #region Properties

    #region BooksCount : int - Количество книг

    ///<summary>Количество книг</summary>
    private int _booksCount;

    ///<summary>Количество книг</summary>
    public int BooksCount
    {
        get => _booksCount;
        private set => Set(ref _booksCount, value);
    }

    #endregion

    #endregion

    #region Commands

    #region ComputeStatisticCommand - Вычислить статистические данные

    private ICommand _computeStatisticCommand;

    ///<summary>Вычислить статистические данные</summary>
    public ICommand ComputeStatisticCommand => _computeStatisticCommand
        ??= new LambdaCommandAsync(OnComputeStatisticCommandExecuted, CanComputeStatisticCommandExecute);

    private bool CanComputeStatisticCommandExecute(object p) => true;

    private async Task OnComputeStatisticCommandExecuted(object p)
    {
        await ComputeDealsStatisticAsync();
    }

    #endregion

    #endregion

    #region Methods

    private async Task ComputeDealsStatisticAsync()
    {
        var deals = _deals.Items;
        var books = _books.Items;

        var bestsellersQuery = deals
            .GroupBy(b => b.Book.Id)
            .Select(d => new {BookId = d.Key, Count = d.Count(), Sum = d.Sum(deal => deal.Price)})
            .OrderByDescending(d => d.Count)
            .Take(5)
            .Join(books,
                deal => deal.BookId,
                book => book.Id,
                (deal, book) => new BestsellerInfo()
                {
                    Book = book,
                    SellCount = deal.Count,
                    SumCost = deal.Sum
                });

        Bestsellers.AddClear(await bestsellersQuery.ToArrayAsync());
    }

    #endregion

    #region Constructors

    public StatisticViewModel(IRepository<Book> books, IRepository<Buyer> buyers, IRepository<Seller> sellers,
        IRepository<Deal> deals)
    {
        _books = books;
        _buyers = buyers;
        _sellers = sellers;
        _deals = deals;
    }

    #endregion
}