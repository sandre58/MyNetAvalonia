// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.MarkupExtensions
{
    public abstract class AbstractGlobalizationExtension : GlobalizationExtensionBase<Binding>
    {
        protected AbstractGlobalizationExtension(bool updateOnCultureChanged, bool updateOnTimeZoneChanged) : base(updateOnCultureChanged, updateOnTimeZoneChanged) { }

        protected AbstractGlobalizationExtension(string path, bool updateOnCultureChanged, bool updateOnTimeZoneChanged) : this(updateOnCultureChanged, updateOnTimeZoneChanged)
            => Path = Path = path;

        protected override Binding CreateBinding() => new() { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };

        [ConstructorArgument("path")]
        public string Path { get => Binding!.Path; set => Binding.Path = value; }

        public string? ElementName { get => Binding.ElementName; set => Binding.ElementName = value; }

        public RelativeSource? RelativeSource { get => Binding.RelativeSource; set => Binding.RelativeSource = value; }

        public object? Source { get => Binding.Source; set => Binding.Source = value; }

        public BindingMode Mode { get => Binding.Mode; set => Binding.Mode = value; }

        public object? ConverterParameter { get => Binding.ConverterParameter; set => Binding.ConverterParameter = value; }

        public IValueConverter? Converter { get => Binding.Converter; set => Binding.Converter = value; }

        protected abstract IValueConverter? CreateConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Binding.Converter == null)
                Binding.Converter = CreateConverter();

            return base.ProvideValue(serviceProvider);
        }
    }
}
