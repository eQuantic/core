namespace eQuantic.Core.Date;

public static class ClockProxy
{
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