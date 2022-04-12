using System;
using System.Windows;
using Bookinist.Services;
using Bookinist.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookinist;

public partial class App
{
    private static IHost _host;
    private static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
    public static IServiceProvider Services => _host.Services;

    public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .AddServices()
        .AddViewModels();

    protected override async void OnStartup(StartupEventArgs e)
    {
        IHost host = Host;

        base.OnStartup(e);

        await host.StartAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        using (Host) await Host.StopAsync();
    }
}