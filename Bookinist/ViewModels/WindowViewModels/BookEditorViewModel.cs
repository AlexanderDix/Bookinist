using System;
using Bookinist.DAL.Entities;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.WindowViewModels;

internal class BookEditorViewModel : ViewModel
{
    #region BookId : int - Идентификатор книги

    ///<summary>Идентификатор книги</summary>
    public int BookId { get; }

    #endregion

    #region Name : string - Название

    ///<summary>Название</summary>
    private string _name;

    ///<summary>Название</summary>
    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }

    #endregion

    public BookEditorViewModel()
        : this(new Book {Id = 1, Name = "Букварь!"})
    {
        if (!App.IsDesignTime)
            throw new InvalidOperationException("Не для рантайма");
    }

    public BookEditorViewModel(Book book)
    {
        BookId = book.Id;
        Name = book.Name;
    }
}