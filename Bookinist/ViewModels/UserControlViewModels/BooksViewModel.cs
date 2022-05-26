using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Infrastructure.DebugServices;
using Bookinist.Interfaces;
using Bookinist.Services;
using Bookinist.Services.Interfaces;
using Bookinist.ViewModels.Base;
using Microsoft.EntityFrameworkCore;

namespace Bookinist.ViewModels.UserControlViewModels;

internal class BooksViewModel : ViewModel
{
    #region Fields

    private readonly IRepository<Book> _booksRepository;
    private readonly IUserDialog _userDialog;
    private readonly CollectionViewSource _booksViewSource;

    #endregion

    #region Properties

    #region Books : ObservableCollection<Book> - Коллекция книг

    ///<summary>Коллекция книг</summary>
    private ObservableCollection<Book> _books;

    ///<summary>Коллекция книг</summary>
    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            if (!Set(ref _books, value)) return;

            _booksViewSource.View.Refresh();
            _booksViewSource.Source = value;

            OnPropertyChanged(nameof(BooksView));
        }
    }

    #endregion

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

    #region SelectedBook : Book - Выбранная книга

    ///<summary>Выбранная книга</summary>
    private Book _selectedBook;

    ///<summary>Выбранная книга</summary>
    public Book SelectedBook
    {
        get => _selectedBook;
        set => Set(ref _selectedBook, value);
    }

    #endregion

    #endregion

    public ICollectionView BooksView => _booksViewSource.View;


    #region Methods

    private void OnBooksFilter(object sender, FilterEventArgs e)
    {
        if (e.Item is not Book book || string.IsNullOrEmpty(BooksFilter)) return;

        if (!book.Name.Contains(BooksFilter))
            e.Accepted = false;
    }

    #endregion

    #region Commands

    #region LoadDataCommand - Команда загрузки данных из репозитория

    private ICommand _loadDataCommand;

    ///<summary>Команда загрузки данных из репозитория</summary>
    public ICommand LoadDataCommand => _loadDataCommand
        ??= new LambdaCommandAsync(OnLoadDataCommandExecuted, CanLoadDataCommandExecute);

    private bool CanLoadDataCommandExecute() => true;

    private async Task OnLoadDataCommandExecuted()
    {
        //Books = (await _booksRepository.Items.ToArrayAsync()).ToObservableCollection();
        Books = new ObservableCollection<Book>(await _booksRepository.Items.ToArrayAsync());
    }

    #endregion

    #region AddBookCommand - Добавление книги

    private ICommand _addBookCommand;

    ///<summary>Добавление книги</summary>
    public ICommand AddBookCommand => _addBookCommand
        ??= new LambdaCommand(OnAddBookCommandExecuted, CanAddBookCommandExecute);

    private bool CanAddBookCommandExecute() => true;

    private void OnAddBookCommandExecuted()
    {
        var newBook = new Book();

        if (!_userDialog.Edit(newBook)) return;

        _books.Add(_booksRepository.Add(newBook));

        SelectedBook = newBook;
    }

    #endregion

    #region RemoveBookCommand - Удаление книги

    private ICommand _removeBookCommand;

    ///<summary>Удаление книги</summary>
    public ICommand RemoveBookCommand => _removeBookCommand
        ??= new LambdaCommand(OnRemoveBookCommandExecuted, CanRemoveBookCommandExecute);

    private bool CanRemoveBookCommandExecute(object p) => p != null || SelectedBook != null;

    private void OnRemoveBookCommandExecuted(object p)
    {
        if (p is not Book book) return;

        if (!_userDialog.ConfirmWarning($"Вы хотите удалить книгу {book.Name}?", "Удаление книги")) return;

        _booksRepository.Remove(book.Id);
        Books.Remove(book);

        if (ReferenceEquals(SelectedBook, book))
            SelectedBook = null;
    }

    #endregion

    #endregion

    #region Constructors

    public BooksViewModel()
        : this(new DebugBooksRepository(), new UserDialogService())
    {
        if (!App.IsDesignTime)
        {
            throw new InvalidOperationException(
                "Данный конструктор предназначен для использования в дизайнере Visual Studio");
        }

        _ = OnLoadDataCommandExecuted();
    }

    public BooksViewModel(IRepository<Book> booksRepository, IUserDialog userDialog)
    {
        _booksRepository = booksRepository;
        _userDialog = userDialog;

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