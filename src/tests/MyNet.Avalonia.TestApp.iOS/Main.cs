// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MyNet.Avalonia.TestApp.iOS;

public static class Application
{
    // This is the main entry point of the application.
    public static void Main(string[] args) =>
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
}
