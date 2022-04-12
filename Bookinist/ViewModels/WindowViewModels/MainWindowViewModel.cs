using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.WindowViewModels;

internal class MainWindowViewModel : ViewModel
{
    #region Fields

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

    #endregion
}