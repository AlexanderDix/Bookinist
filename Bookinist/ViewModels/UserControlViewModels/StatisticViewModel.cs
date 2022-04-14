using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.UserControlViewModels;

internal class StatisticViewModel : ViewModel
{
    private readonly IRepository<Book> _books;
    private readonly IRepository<Buyer> _buyers;
    private readonly IRepository<Seller> _sellers;

    public StatisticViewModel(IRepository<Book> books, IRepository<Buyer> buyers, IRepository<Seller> sellers)
    {
        _books = books;
        _buyers = buyers;
        _sellers = sellers;
    }
}