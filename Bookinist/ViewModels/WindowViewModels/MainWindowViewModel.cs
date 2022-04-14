using System.Windows.Input;
using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Services.Interfaces;
using Bookinist.ViewModels.Base;
using Bookinist.ViewModels.UserControlViewModels;

namespace Bookinist.ViewModels.WindowViewModels;

internal class MainWindowViewModel : ViewModel
{
    #region Fields

    private readonly IRepository<Book> _books;
    private readonly IRepository<Seller> _sellers;
    private readonly IRepository<Buyer> _buyers;
    private readonly ISalesService _salesService;

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

    #region CurrentModel : ViewModel - Текущая модель представление

    ///<summary>Текущая модель представление</summary>
    private ViewModel _currentModel;

    ///<summary>Текущая модель представление</summary>
    public ViewModel CurrentModel
    {
        get => _currentModel;
        private set => Set(ref _currentModel, value);
    }

    #endregion

    #endregion

    #region Commands

    #region ShowBooksViewCommand - Команда открывающая представление книг

    private ICommand _showBooksViewCommand;

    ///<summary>Команда открывающая представление книг</summary>
    public ICommand ShowBooksViewCommand => _showBooksViewCommand
        ??= new LambdaCommand(OnShowBooksViewCommandExecuted, CanShowBooksViewCommandExecute);

    private bool CanShowBooksViewCommandExecute(object p) => true;

    private void OnShowBooksViewCommandExecuted(object p)
    {
        CurrentModel = new BooksViewModel(_books);
    }

    #endregion

    #region ShowBuyersViewCommand - Команда открывающая представление покупателей

    private ICommand _showBuyersViewCommand;

    ///<summary>Команда открывающая представление покупателей</summary>
    public ICommand ShowBuyersViewCommand => _showBuyersViewCommand
        ??= new LambdaCommand(OnShowBuyersViewCommandExecuted, CanShowBuyersViewCommandExecute);

    private bool CanShowBuyersViewCommandExecute(object p) => true;

    private void OnShowBuyersViewCommandExecuted(object p)
    {
        CurrentModel = new BuyersViewModel(_buyers);
    }

    #endregion

    #region ShowStatisticViewCommand - Команда открывающая представление с статистикой

    private ICommand _showStatisticViewCommand;

    ///<summary>Команда открывающая представление с статистикой</summary>
    public ICommand ShowStatisticViewCommand => _showStatisticViewCommand
        ??= new LambdaCommand(OnShowStatisticViewCommandExecuted, CanShowStatisticViewCommandExecute);

    private bool CanShowStatisticViewCommandExecute(object p) => true;

    private void OnShowStatisticViewCommandExecuted(object p)
    {
        CurrentModel = new StatisticViewModel(
            _books, _buyers, _sellers);
    }

    #endregion

    #endregion

    #region Methods

    //private async void Test()
    //{
    //    var dealsCount = _salesService.Deals.Count();

    //    Book book = await _books.GetAsync(5);
    //    Buyer buyer = await _buyers.GetAsync(3);
    //    Seller seller = await _sellers.GetAsync(7);

    //    var deal = _salesService.MakeADeal(book.Name, seller, buyer, 100m);

    //    var dealsCountTwo = _salesService.Deals.Count();
    //}

    #endregion

    #region Constructors

    public MainWindowViewModel(IRepository<Book> books, IRepository<Seller> sellers,
        IRepository<Buyer> buyers, ISalesService salesService)
    {
        _books = books;
        _sellers = sellers;
        _buyers = buyers;
        _salesService = salesService;
    }

    #endregion
}