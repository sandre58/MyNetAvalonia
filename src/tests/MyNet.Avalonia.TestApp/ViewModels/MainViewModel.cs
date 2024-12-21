// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Commands;
using System.Windows.Input;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Shell;
using MyNet.UI.Theming;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.TestApp.ViewModels
{
    internal class MainViewModel : MainWindowViewModelBase
    {
        public ICommand SetPrimaryColorCommand { get; }

        public ICommand SetAccentColorCommand { get; }

        public MainViewModel(INotificationsManager notificationsManager, IAppCommandsService appCommandsService)
            : base(notificationsManager, appCommandsService, AppBusyManager.MainBusyService)
        {
            SetPrimaryColorCommand = CommandsManager.Create(() => ThemeManager.ApplyPrimaryColor(RandomGenerator.Color()));
            SetAccentColorCommand = CommandsManager.Create(() => ThemeManager.ApplyAccentColor(RandomGenerator.Color()));
        }
    }
}
