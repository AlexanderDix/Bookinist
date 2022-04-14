using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.ViewModels.Base;

namespace Bookinist.ViewModels.UserControlViewModels;

internal class BuyersViewModel : ViewModel
{
    private readonly IRepository<Buyer> _buyers;

    public BuyersViewModel(IRepository<Buyer> buyers)
    {
        _buyers = buyers;
    }
}