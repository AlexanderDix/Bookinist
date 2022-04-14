using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.UserControlViewModels;

internal class BooksViewModel : ViewModel
{
    private readonly IRepository<Book> _booksRepository;

    public BooksViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;
    }
}