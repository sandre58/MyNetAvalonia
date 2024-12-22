// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System;
using Avalonia;
using Avalonia.Data;
using System.Reactive.Disposables;

namespace MyNet.Avalonia.Extensions
{
    public class ResultDisposable : IDisposable
    {
        [CompilerGenerated]
        private readonly IDisposable? _disposable;

        public bool Result { get; }

        public ResultDisposable(IDisposable? disposable, bool result)
        {
            _disposable = disposable;
            Result = result;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public static class AvaloniaObjectExtensions
    {
        public static ResultDisposable TryBind(this AvaloniaObject obj, AvaloniaProperty property, IBinding? binding)
            => binding == null
                ? new ResultDisposable(Disposable.Empty, result: false)
                : new ResultDisposable(obj.Bind(property, binding), result: true);
    }
}
