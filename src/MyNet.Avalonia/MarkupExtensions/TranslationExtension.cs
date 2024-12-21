// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using Avalonia.Data;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Converters;
using MyNet.Humanizer;

namespace MyNet.Avalonia.MarkupExtensions
{
    public class TranslationExtension : AbstractGlobalizationExtension
    {
        public TranslationExtension() : base(true, false) { }

        public TranslationExtension(string path) : this() => Path = path;

        protected override Binding CreateBinding() => new();

        public string? Format { get => Binding.ConverterParameter?.ToString(); set => Binding.ConverterParameter = value; }

        public bool Pluralize { get; set; }

        public bool Abbreviate { get; set; }

        public LetterCasing Casing { get; set; } = LetterCasing.Normal;

        protected override IValueConverter CreateConverter() => new StringConverter(Casing, Pluralize, Abbreviate);
    }
}
