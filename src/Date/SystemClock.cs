using System;

namespace eQuantic.Core.Date;

/// <summary>
/// Provides a default implementation of <see cref="IClock"/> that returns the system's current date and time.
/// </summary>
public class SystemClock : IClock
{
	/// <summary>
	/// Initializes a new instance of the <see cref="SystemClock"/> class.
	/// </summary>
	internal SystemClock()
	{
	}

	/// <summary>
	/// Gets the current system date and time.
	/// </summary>
	/// <value>The current <see cref="DateTime"/> from <see cref="DateTime.Now"/>.</value>
	public DateTime Now
	{
		get { return DateTime.Now; }
	}

} // class SystemClock

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
