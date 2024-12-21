// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;

namespace MyNet.Avalonia.TestApp.Browser
{
    internal static partial class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Method args")]
        private static Task Main(string[] args) => BuildAvaloniaApp()
                .WithInterFont()
                .StartBrowserAppAsync("out");

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>();
    }
}
