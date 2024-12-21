// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia.Busy;
using MyNet.Avalonia.Commands;
using MyNet.Avalonia.Schedulers;
using MyNet.Avalonia.Services;
using MyNet.Avalonia.TestApp.ViewModels;
using MyNet.Avalonia.TestApp.Views;
using MyNet.Avalonia.Theming;
using MyNet.UI.Busy;
using MyNet.UI.Commands;
using MyNet.UI.Locators;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.Theming;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.TestApp;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        // Register all the services needed for the application to run
        var collection = new ServiceCollection();
        RegisterServices(collection);
        RegisterViewModels(collection);

        // Creates a ServiceProvider containing services from the provided IServiceCollection
        var services = collection.BuildServiceProvider();

        InitializeServices(services);

        var vm = ViewModelManager.Get<MainViewModel>();
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow() { DataContext = vm };
                break;
            case ISingleViewApplicationLifetime singleView:
                singleView.MainView = new MainView() { DataContext = vm };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void RegisterServices(ServiceCollection collection)
        => collection.AddSingleton<ILogger, Utilities.Logging.NLog.Logger>()
                     .AddSingleton<IViewModelResolver, ViewModelResolver>()
                     .AddSingleton<IViewModelLocator, ViewModelLocator>(x => new ViewModelLocator(x))
                     .AddSingleton<IViewLocator, ViewLocator>()
                     .AddSingleton<IViewResolver, ViewResolver>()
                     .AddSingleton<IThemeService, ThemeService>()
                     .AddSingleton<INotificationsManager, NotificationsManager>()
                     //.AddSingleton<INavigationService, NavigationService>()
                     //.AddSingleton<IToasterService, ToasterService>()
                     //.AddSingleton<IDialogService, OverlayDialogService>()
                     .AddScoped<IBusyServiceFactory, BusyServiceFactory>()
                     //.AddScoped<IMessageBoxFactory, MessageBoxFactory>()
                     .AddScoped<IScheduler, AvaloniaScheduler>(_ => AvaloniaScheduler.Current)
                     .AddScoped<ICommandFactory, AvaloniaCommandFactory>()
                     .AddScoped<IAppCommandsService, AppCommandsService>()
                     ;

    private static void InitializeServices(ServiceProvider services)
    {
        // Logging
        Utilities.Logging.NLog.Logger.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/config/NLog.config"));

        LogManager.Initialize(services.GetRequiredService<ILogger>());
        ViewModelManager.Initialize(services.GetRequiredService<IViewModelResolver>(), services.GetRequiredService<IViewModelLocator>());
        ViewManager.Initialize(services.GetRequiredService<IViewResolver>(), services.GetRequiredService<IViewLocator>());
        ThemeManager.Initialize(services.GetRequiredService<IThemeService>());
        //NavigationManager.Initialize(navigationService, viewModelLocator);
        //ToasterManager.Initialize(toasterService);
        //DialogManager.Initialize(dialogService, messageBoxFactory, viewResolver, viewLocator, viewModelLocator);
        var busyFactory = services.GetRequiredService<IBusyServiceFactory>();
        BusyManager.Initialize(busyFactory);
        AppBusyManager.Initialize(busyFactory);
        CommandsManager.Initialize(services.GetRequiredService<ICommandFactory>());
        UI.Threading.Scheduler.Initialize(services.GetRequiredService<IScheduler>());
    }

    private static void RegisterViewModels(ServiceCollection collection)
        => collection.AddSingleton<MainViewModel>();
}
