using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookinist.DAL.Entities;
using Bookinist.Interfaces;

namespace Bookinist.Infrastructure.DebugServices;

internal class DebugBooksRepository : IRepository<Book>
{
    public DebugBooksRepository()
    {
        var books = Enumerable.Range(1, 100)
            .Select(i => new Book
            {
                Id = i,
                Name = $"Книга {i}"
            })
            .ToArray();

        var categories = Enumerable.Range(1, 10)
            .Select(i => new Category
            {
                Id = i,
                Name = $"Категория {i}"
            })
            .ToArray();

        foreach (Book book in books)
        {
            Category category = categories[book.Id % categories.Length];
            category.Books.Add(book);
            book.Category = category;
        }

        Items = books.AsQueryable();
    }

    public IQueryable<Book> Items { get; }

    public Book Get(int id) => throw new System.NotImplementedException();

    public Task<Book> GetAsync(int id, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();

    public Book Add(Book item) => throw new System.NotImplementedException();

    public Task<Book> AddAsync(Book item, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();

    public void Update(Book item)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateAsync(Book item, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();

    public void Remove(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task RemoveAsync(int id, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
}