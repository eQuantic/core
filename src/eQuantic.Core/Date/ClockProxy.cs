namespace eQuantic.Core.Date;

/// <summary>
/// Provides a static proxy for accessing clock functionality with thread-safe lazy initialization.
/// </summary>
public static class ClockProxy
{
    /// <summary>
    /// Gets or sets the current clock instance. Defaults to SystemClock if not set.
    /// </summary>
    public static IClock Clock
    {
        get
        {
            if (_clock != null) return _clock;
            lock (Mutex)
            {
                if (_clock == null)
                {
                    _clock = new SystemClock();
                }
            }
            return _clock;
        }
        set
        {
            lock (Mutex)
            {
                _clock = value;
            }
        }
    }

    private static readonly object Mutex = new object();
    private static volatile IClock _clock;

}