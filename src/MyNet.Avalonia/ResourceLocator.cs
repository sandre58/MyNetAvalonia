// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using MyNet.Avalonia.Resources;
using MyNet.UI.Resources;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia
{
    public static class ResourceLocator
    {
        private static bool _isInitialized;

        public static Dictionary<Color, string> ColorResourcesDictionary { get; private set; } = [];

        public static void Initialize()
        {
            if (_isInitialized) return;

            // Common Resources
            TranslationService.RegisterResources(nameof(UiResources), UiResources.ResourceManager);
            TranslationService.RegisterResources(nameof(MessageResources), MessageResources.ResourceManager);
            TranslationService.RegisterResources(nameof(FormatResources), FormatResources.ResourceManager);
            TranslationService.RegisterResources(nameof(ColorResources), ColorResources.ResourceManager);

            Humanizer.ResourceLocator.Initialize();

            GlobalizationService.Current.CultureChanged += (sender, e) => FillColorResourcesDictionary();
            _isInitialized = true;
        }

        private static void FillColorResourcesDictionary()
        {
            ColorResourcesDictionary = [];
            var resourceSet = ColorResources.ResourceManager.GetResourceSet(GlobalizationService.Current.Culture, true, true);

            if (resourceSet is not null)
            {
                foreach (var entry in resourceSet.OfType<DictionaryEntry>())
                {
                    try
                    {
                        if (Color.Parse(entry.Key.ToString().OrEmpty()) is Color color)
                        {
                            ColorResourcesDictionary.Add(color, entry.Value!.ToString()!);
                        }
                    }
                    catch (Exception)
                    {
                        LogManager.Warning($"{entry.Key} is not a valid color key");
                    }
                }
            }
        }
    }
}
