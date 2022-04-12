using Bookinist.ViewModels.WindowViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.ViewModels;

internal class ViewModelLocator
{
    public static MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
}