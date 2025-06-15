#if !NETSTANDARD1_6
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Task"/> and <see cref="Task{T}"/> to handle culture preservation.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Creates a culture-aware awaiter that preserves the current culture and UI culture across async operations.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to wrap.</param>
    /// <returns>A culture-aware awaiter that preserves the current thread's culture.</returns>
    public static TaskExtensions.CultureAwaiter<T> WithCurrentCulture<T>(this Task<T> task)
    {
        return new TaskExtensions.CultureAwaiter<T>(task);
    }

    /// <summary>
    /// Creates a culture-aware awaiter that preserves the current culture and UI culture across async operations.
    /// </summary>
    /// <param name="task">The task to wrap.</param>
    /// <returns>A culture-aware awaiter that preserves the current thread's culture.</returns>
    public static TaskExtensions.CultureAwaiter WithCurrentCulture(this Task task)
    {
        return new TaskExtensions.CultureAwaiter(task);
    }

    /// <summary>
    /// Provides a culture-aware awaiter for Task&lt;T&gt; that preserves the current culture and UI culture.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    public struct CultureAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly Task<T> _task;

        /// <summary>
        /// Gets a value indicating whether the underlying task has completed.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return this._task.IsCompleted;
            }
        }

        /// <summary>
        /// Initializes a new instance of the CultureAwaiter struct.
        /// </summary>
        /// <param name="task">The task to wrap with culture preservation.</param>
        public CultureAwaiter(Task<T> task)
        {
            this._task = task;
        }

        /// <summary>
        /// Gets the awaiter for this culture-aware task.
        /// </summary>
        /// <returns>The culture awaiter instance.</returns>
        public TaskExtensions.CultureAwaiter<T> GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Gets the result of the underlying task.
        /// </summary>
        /// <returns>The task result.</returns>
        public T GetResult()
        {
            return this._task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Schedules the continuation action to be invoked when the task completes.
        /// </summary>
        /// <param name="continuation">The action to invoke when the task completes.</param>
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Schedules the continuation action to be invoked when the task completes, preserving the current culture.
        /// </summary>
        /// <param name="continuation">The action to invoke when the task completes.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            this._task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted((Action)(() =>
            {
                CultureInfo currentCulture1 = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentUiCulture1 = Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                try
                {
                    continuation();
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = currentCulture1;
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture1;
                }
            }));
        }
    }

    /// <summary>
    /// Provides a culture-aware awaiter for Task that preserves the current culture and UI culture.
    /// </summary>
    public struct CultureAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly Task _task;

        /// <summary>
        /// Gets a value indicating whether the underlying task has completed.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return this._task.IsCompleted;
            }
        }

        /// <summary>
        /// Initializes a new instance of the CultureAwaiter struct.
        /// </summary>
        /// <param name="task">The task to wrap with culture preservation.</param>
        public CultureAwaiter(Task task)
        {
            this._task = task;
        }

        /// <summary>
        /// Gets the awaiter for this culture-aware task.
        /// </summary>
        /// <returns>The culture awaiter instance.</returns>
        public TaskExtensions.CultureAwaiter GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Gets the result of the underlying task.
        /// </summary>
        public void GetResult()
        {
            this._task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Schedules the continuation action to be invoked when the task completes.
        /// </summary>
        /// <param name="continuation">The action to invoke when the task completes.</param>
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Schedules the continuation action to be invoked when the task completes, preserving the current culture.
        /// </summary>
        /// <param name="continuation">The action to invoke when the task completes.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
            this._task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted((Action)(() =>
            {
                CultureInfo currentCulture1 = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentUiCulture1 = Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentCulture = currentCulture;
                Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                try
                {
                    continuation();
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = currentCulture1;
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture1;
                }
            }));
        }
    }
}
#endif