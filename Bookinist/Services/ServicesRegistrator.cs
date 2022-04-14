using Bookinist.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bookinist.Services;

internal static class ServicesRegistrator
{
    public static IServiceCollection AddServices(this IServiceCollection services) => services
        .AddTransient<IUserDialog, UserDialogService>()
        .AddTransient<ISalesService, SalesService>()
    ;
}