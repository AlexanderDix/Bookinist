using System.Linq;
using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.WindowViewModels;

internal class MainWindowViewModel : ViewModel
{
    #region Fields

    private readonly IRepository<Book> _booksRepository;

    #endregion

    #region Properties

    #region Title : string - Заголовок окна

    ///<summary>Заголовок окна</summary>
    private string _title = "Bookinist";

    ///<summary>Заголовок окна</summary>
    public string Title
    {
        get => _title;
        set => Set(ref _title, value);
    }

    #endregion

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion

    #region Constructors

    public MainWindowViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;

        var books = _booksRepository.Items.Take(10).ToArray();
    }

    #endregion
}