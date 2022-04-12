using Bookinist.ViewModels.WindowViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.ViewModels;

internal class ViewModelLocator
{
    public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
}