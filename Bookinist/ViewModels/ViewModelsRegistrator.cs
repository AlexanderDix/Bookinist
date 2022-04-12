using Bookinist.ViewModels.WindowViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.ViewModels;

internal static class ViewModelsRegistrator
{
    public static IServiceCollection AddViewModels(this IServiceCollection services) => services
        .AddSingleton<MainWindowViewModel>();
}