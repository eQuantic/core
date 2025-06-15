using System;

namespace eQuantic.Core.Date;

/// <summary>
/// Defines a contract for providing the current date and time, enabling testability by abstracting the system clock.
/// </summary>
/// <remarks>
/// This interface allows for easy mocking of the current time in unit tests.
/// See http://stackoverflow.com/questions/43711/whats-a-good-way-to-overwrite-datetime-now-during-testing
/// </remarks>
public interface IClock
{
	/// <summary>
	/// Gets the current date and time.
	/// </summary>
	/// <value>The current <see cref="DateTime"/>.</value>
	DateTime Now { get; }

} // interface IClock

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
