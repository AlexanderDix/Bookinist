using System.Collections.Generic;
using System.Threading.Tasks;
using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.Services;

internal class SalesService : ISalesService
{
    private readonly IRepository<Book> _books;
    private readonly IRepository<Deal> _deals;

    public IEnumerable<Deal> Deals => _deals.Items;

    public SalesService(IRepository<Book> books, IRepository<Deal> deals)
    {
        _books = books;
        _deals = deals;
    }

    public async Task<Deal> MakeADeal(string bookName, Seller seller, Buyer buyer, decimal price)
    {
        Book book = await _books.Items.FirstOrDefaultAsync(b => b.Name == bookName).ConfigureAwait(false);
        if (book is null) return null;

        var deal = new Deal
        {
            Book = book,
            Seller = seller,
            Buyer = buyer,
            Price = price
        };

        return await _deals.AddAsync(deal);
    }
}