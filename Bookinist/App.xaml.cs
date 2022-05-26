using System;
using System.Linq;
using System.Windows;
using Bookinist.Data;
using Bookinist.Services;
using Bookinist.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookinist;

public partial class App
{
    #region Fields

    private static IHost _host;

    #endregion

    #region Properties

    private static IHost Host => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

    public static IServiceProvider Services => _host.Services;

    public static Window ActiveWindow => Current.Windows
        .OfType<Window>()
        .FirstOrDefault(w => w.IsActive);

    public static Window FocusedWindow => Current.Windows
        .OfType<Window>()
        .FirstOrDefault(w => w.IsFocused);

    public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

    public static bool IsDesignTime { get; private set; } = true;

    private static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .AddDataBase(host.Configuration.GetSection("Database"))
        .AddServices()
        .AddViewModels();

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

    #endregion

    #region Methods

    protected override async void OnStartup(StartupEventArgs e)
    {
        IsDesignTime = false;

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

    #endregion
}