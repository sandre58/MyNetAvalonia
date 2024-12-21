// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Humanizer;
using MyNet.Observable.Translatables;

namespace MyNet.Avalonia.MarkupExtensions
{
    public class ResourceExtension : MarkupExtension
    {
        static ResourceExtension() => ResourceLocator.Initialize();

        public ResourceExtension(string key) => Key = key;

        public ResourceExtension(string key, string filename)
        {
            Key = key;
            Filename = filename;
        }

        [ConstructorArgument("key")]
        public string Key { get; set; }

        [ConstructorArgument("filename")]
        public string? Filename { get; set; }

        public LetterCasing Casing { get; set; } = LetterCasing.Normal;

        public override object ProvideValue(IServiceProvider serviceProvider)
            => new Binding(nameof(TranslatableString.Value))
            {
                Source = new TranslatableString(Key, Casing, Filename)
            };
    }

}
