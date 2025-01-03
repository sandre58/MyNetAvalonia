﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using Avalonia.Threading;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Schedulers
{
    /// <summary>
    /// Represents an object that schedules units of work on a <see cref="Windows.Threading.Dispatcher"/>.
    /// </summary>
    /// <remarks>
    /// This scheduler type is typically used indirectly through the <see cref="Linq.DispatcherObservable.ObserveOnDispatcher&lt;TSource&gt;(IObservable&lt;TSource&gt;)"/> and <see cref="Linq.DispatcherObservable.SubscribeOnDispatcher&lt;TSource&gt;(IObservable&lt;TSource&gt;)"/> methods that use the Dispatcher on the calling thread.
    /// </remarks>
    public class AvaloniaScheduler : LocalScheduler, ISchedulerPeriodic
    {
        private static AvaloniaScheduler _current = null!;

        /// <summary>
        /// Gets the scheduler that schedules work on the <see cref="Windows.Threading.Dispatcher"/> for the current thread.
        /// </summary>
        public static AvaloniaScheduler Current => _current ??= new AvaloniaScheduler();

        /// <summary>
        /// Constructs a WpfScheduler that schedules units of work on the given <see cref="Windows.Threading.Dispatcher"/>.
        /// </summary>
        public AvaloniaScheduler() => Priority = DispatcherPriority.Render;

        /// <summary>
        /// Constructs a WpfScheduler that schedules units of work on the given <see cref="Windows.Threading.Dispatcher"/> at the given priority.
        /// </summary>
        /// <param name="priority">Priority at which units of work are scheduled.</param>
        public AvaloniaScheduler(DispatcherPriority priority) => Priority = priority;

        /// <summary>
        /// Gets the priority at which work items will be dispatched.
        /// </summary>
        public DispatcherPriority Priority { get; }

        /// <summary>
        /// Schedules an action to be executed on the dispatcher.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            var d = new SingleAssignmentDisposable();

            Dispatcher.UIThread.Invoke(() =>
                {
                    if (!d.IsDisposed)
                    {
                        Thread.CurrentThread.CurrentCulture = GlobalizationService.Current.Culture;
                        Thread.CurrentThread.CurrentUICulture = GlobalizationService.Current.Culture;

                        d.Disposable = action(this, state);
                    }
                }
                , Priority
            );

            return d;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime on the dispatcher, using a <see cref="DispatcherTimer"/> object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks == 0)
                return Schedule(state, action);

            var d = new MultipleAssignmentDisposable();

            var timer = new DispatcherTimer(Priority);

            timer.Tick += (s, e) =>
            {
                var t = Interlocked.Exchange(ref timer, default);
                if (t != null)
                {
                    try
                    {
                        Thread.CurrentThread.CurrentCulture = GlobalizationService.Current.Culture;
                        Thread.CurrentThread.CurrentUICulture = GlobalizationService.Current.Culture;

                        d.Disposable = action(this, state);
                    }
                    finally
                    {
                        t.Stop();
                        action = (_, _) => Disposable.Empty;
                    }
                }
            };

            timer.Interval = dt;
            timer.Start();

            d.Disposable = Disposable.Create(() =>
            {
                var t = Interlocked.Exchange(ref timer, default);
                if (t != null)
                {
                    t.Stop();
                    action = (_, _) => Disposable.Empty;
                }
            });

            return d;
        }

        /// <summary>
        /// Schedules a periodic piece of work on the dispatcher, using a <see cref="DispatcherTimer"/> object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(period, TimeSpan.Zero);
            ArgumentNullException.ThrowIfNull(action);

            var timer = new DispatcherTimer(Priority);

            var state1 = state;

            timer.Tick += (s, e) =>
            {
                Thread.CurrentThread.CurrentCulture = GlobalizationService.Current.Culture;
                Thread.CurrentThread.CurrentUICulture = GlobalizationService.Current.Culture;

                state1 = action(state1);
            };

            timer.Interval = period;
            timer.Start();

            return Disposable.Create(() =>
            {
                var t = Interlocked.Exchange(ref timer, default);
                if (t != null)
                {
                    t.Stop();
                    action = _ => _;
                }
            });
        }
    }
}
