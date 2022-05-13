using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.DebugServices;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.UserControlViewModels;

internal class BooksViewModel : ViewModel
{
    #region Fields

    private readonly IRepository<Book> _booksRepository;
    private readonly CollectionViewSource _booksViewSource;

    #endregion

    #region Properties

    public IEnumerable<Book> Books => _booksRepository.Items;

    #region BooksFilter : string - Искомое значение

    ///<summary>Искомое значение</summary>
    private string _booksFilter;

    ///<summary>Искомое значение</summary>
    public string BooksFilter
    {
        get => _booksFilter;
        set
        {
            if (Set(ref _booksFilter, value))
                _booksViewSource.View.Refresh();
        }
    }

    #endregion

    public ICollectionView BooksView => _booksViewSource.View;

    #endregion

    #region Methods

    private void OnBooksFilter(object sender, FilterEventArgs e)
    {
        if (e.Item is not Book book || string.IsNullOrEmpty(BooksFilter)) return;

        if (!book.Name.Contains(BooksFilter))
            e.Accepted = false;
    }

    #endregion

    #region Constructors

    public BooksViewModel()
        : this(new DebugBooksRepository())
    {
        if (!App.IsDesignTime)
        {
            throw new InvalidOperationException(
                "Данный конструктор предназначен для использования в дизайнере Visual Studio");
        }
    }

    public BooksViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;

        _booksViewSource = new CollectionViewSource
        {
            Source = _booksRepository.Items.ToArray(),
            SortDescriptions =
            {
                new SortDescription(nameof(Book.Name), ListSortDirection.Ascending)
            }
        };

        _booksViewSource.Filter += OnBooksFilter;
    }

    #endregion
}