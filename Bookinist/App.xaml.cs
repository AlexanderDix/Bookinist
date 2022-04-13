using System;
using System.Windows;
using Bookinist.Data;
using Bookinist.Services;
using Bookinist.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookinist;

public partial class App
{
    private static IHost _host;
    private static IHost Host => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    public static IServiceProvider Services => _host.Services;

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .AddDataBase(host.Configuration.GetSection("Database"))
        .AddServices()
        .AddViewModels();

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

    protected override async void OnStartup(StartupEventArgs e)
    {
        IHost host = Host;

        using (IServiceScope scope = Services.CreateScope())
            await scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync();

        base.OnStartup(e);

        await host.StartAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        using (Host) await Host.StopAsync();
    }
}