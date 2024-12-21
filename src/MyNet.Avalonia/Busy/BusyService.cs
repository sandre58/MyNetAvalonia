// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MyNet.Observable;
using MyNet.UI.Busy;

namespace MyNet.Avalonia.Busy
{
    public class BusyService : ObservableObject, IBusyService
    {
        public bool IsBusy => false;

        public void Resume() { }

        TBusy? IBusyService.GetCurrent<TBusy>() where TBusy : class => Activator.CreateInstance<TBusy>();

        TBusy IBusyService.Wait<TBusy>() => Activator.CreateInstance<TBusy>();

        Task IBusyService.WaitAsync<TBusy>(Action<TBusy> action) => Task.CompletedTask;

        Task IBusyService.WaitAsync<TBusy>(Func<TBusy, Task> action) => Task.CompletedTask;
    }
}
