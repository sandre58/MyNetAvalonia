// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Services;

namespace MyNet.Avalonia.Services
{
    public class AppCommandsService : IAppCommandsService
    {
        public void Exit() => System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
    }
}
