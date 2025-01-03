﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Commands
{
    public class AvaloniaCommandFactory : ICommandFactory
    {
        public ICommand Create(Action execute) => new RelayCommand(execute);

        public ICommand Create(Action execute, Func<bool> canExectute) => new RelayCommand(execute, canExectute);

        public ICommand Create<T>(Action<T?> execute) => new RelayCommand<T>(execute);

        public ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExectute) => new RelayCommand<T>(execute, canExectute);
    }
}
