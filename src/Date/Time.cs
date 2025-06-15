using System;

namespace eQuantic.Core.Date;

/// <summary>
/// Represents a time of day without date information, providing time-only operations and comparisons.
/// </summary>
public struct Time : IComparable, IComparable<Time>, IEquatable<Time>
{
	/// <summary>
	/// Initializes a new instance of the Time struct from a DateTime value, extracting only the time component.
	/// </summary>
	/// <param name="dateTime">The DateTime value to extract the time from.</param>
	public Time( DateTime dateTime )
	{
		duration = dateTime.TimeOfDay;
	} // Time

	/// <summary>
	/// Initializes a new instance of the Time struct from a TimeSpan duration.
	/// </summary>
	/// <param name="duration">The TimeSpan duration to convert to time.</param>
	public Time( TimeSpan duration ) :
		this( Math.Abs( duration.Hours ), Math.Abs( duration.Minutes ),
			Math.Abs( duration.Seconds ), Math.Abs( duration.Milliseconds ) )
	{
	} // Time

	/// <summary>
	/// Initializes a new instance of the Time struct with the specified hour, minute, second, and millisecond.
	/// </summary>
	/// <param name="hour">The hour (0-24). Defaults to 0.</param>
	/// <param name="minute">The minute (0-59). Defaults to 0.</param>
	/// <param name="second">The second (0-59). Defaults to 0.</param>
	/// <param name="millisecond">The millisecond (0-999). Defaults to 0.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when any parameter is out of valid range.</exception>
	public Time( int hour = 0, int minute = 0, int second = 0, int millisecond = 0 )
	{
		if ( hour < 0 || hour > TimeSpec.HoursPerDay )
		{
			throw new ArgumentOutOfRangeException( "hour" );
		}
		if ( hour == TimeSpec.HoursPerDay )
		{
			if ( minute > 0 )
			{
				throw new ArgumentOutOfRangeException( "minute" );
			}
			if ( second > 0 )
			{
				throw new ArgumentOutOfRangeException( "second" );
			}
			if ( millisecond > 0 )
			{
				throw new ArgumentOutOfRangeException( "millisecond" );
			}
		}
		if ( minute < 0 || minute >= TimeSpec.MinutesPerHour )
		{
			throw new ArgumentOutOfRangeException( "minute" );
		}
		if ( second < 0 || second >= TimeSpec.SecondsPerMinute )
		{
			throw new ArgumentOutOfRangeException( "second" );
		}
		if ( millisecond < 0 || millisecond >= TimeSpec.MillisecondsPerSecond )
		{
			throw new ArgumentOutOfRangeException( "millisecond" );
		}

		duration = new TimeSpan( 0, hour, minute, second, millisecond );
	} // Time

	/// <summary>
	/// Gets the hour component of the time.
	/// </summary>
	public int Hour
	{
		get { return duration.Hours; }
	} // Hour

	/// <summary>
	/// Gets the minute component of the time.
	/// </summary>
	public int Minute
	{
		get { return duration.Minutes; }
	} // Minute

	/// <summary>
	/// Gets the second component of the time.
	/// </summary>
	public int Second
	{
		get { return duration.Seconds; }
	} // Second

	/// <summary>
	/// Gets the millisecond component of the time.
	/// </summary>
	public int Millisecond
	{
		get { return duration.Milliseconds; }
	} // Millisecond

	/// <summary>
	/// Gets the underlying TimeSpan duration representing this time.
	/// </summary>
	public TimeSpan Duration
	{
		get { return duration; }
	} // Duration

	/// <summary>
	/// Gets a value indicating whether this time represents zero (00:00:00.000).
	/// </summary>
	public bool IsZero
	{
		get { return duration.Equals( TimeSpan.Zero ); }
	} // IsZero

	/// <summary>
	/// Gets a value indicating whether this time represents a full day (24:00:00.000).
	/// </summary>
	public bool IsFullDay
	{
		get { return (int)duration.TotalHours == TimeSpec.HoursPerDay; }
	} // IsFullDay

	/// <summary>
	/// Gets a value indicating whether this time represents either zero or a full day.
	/// </summary>
	public bool IsFullDayOrZero
	{
		get { return IsFullDay || IsZero; }
	} // IsFullDayOrZero

	/// <summary>
	/// Gets the number of ticks that represent this time.
	/// </summary>
	public long Ticks
	{
		get { return duration.Ticks; }
	} // Ticks

	/// <summary>
	/// Gets the total number of hours represented by this time, including fractional hours.
	/// </summary>
	public double TotalHours
	{
		get { return duration.TotalHours; }
	} // TotalHours

	/// <summary>
	/// Gets the total number of minutes represented by this time, including fractional minutes.
	/// </summary>
	public double TotalMinutes
	{
		get { return duration.TotalMinutes; }
	} // TotalMinutes

	/// <summary>
	/// Gets the total number of seconds represented by this time, including fractional seconds.
	/// </summary>
	public double TotalSeconds
	{
		get { return duration.TotalSeconds; }
	} // TotalSeconds

	/// <summary>
	/// Gets the total number of milliseconds represented by this time, including fractional milliseconds.
	/// </summary>
	public double TotalMilliseconds
	{
		get { return duration.TotalMilliseconds; }
	} // TotalMilliseconds

	/// <summary>
	/// Compares this instance to another Time instance and returns an indication of their relative values.
	/// </summary>
	/// <param name="other">The Time instance to compare with this instance.</param>
	/// <returns>A signed integer that indicates the relative order of the objects being compared.</returns>
	public int CompareTo( Time other )
	{
		return duration.CompareTo( other.duration );
	} // CompareTo

	/// <summary>
	/// Compares this instance to a specified object and returns an indication of their relative values.
	/// </summary>
	/// <param name="obj">An object to compare, or null.</param>
	/// <returns>A signed integer that indicates the relative order of the objects being compared.</returns>
	public int CompareTo( object obj )
	{
		return duration.CompareTo( ((Time)obj).duration );
	} // CompareTo

	/// <summary>
	/// Returns a value indicating whether this instance is equal to the specified Time instance.
	/// </summary>
	/// <param name="other">The Time instance to compare with this instance.</param>
	/// <returns>true if the other parameter equals the value of this instance; otherwise, false.</returns>
	public bool Equals( Time other )
	{
		return duration.Equals( other.duration );
	} // Equals

	/// <summary>
	/// Converts the value of this instance to its equivalent string representation in HH:mm:ss.fff format.
	/// </summary>
	/// <returns>A string representation of this Time instance.</returns>
	public override string ToString()
	{
		return ( (int)TotalHours ).ToString( "00" ) + ":" + Minute.ToString( "00" ) +
		       ":" + Second.ToString( "00" ) + "." + Millisecond.ToString( "000" );
	} // ToString

	/// <summary>
	/// Returns a value indicating whether this instance is equal to a specified object.
	/// </summary>
	/// <param name="obj">An object to compare with this instance.</param>
	/// <returns>true if obj is a Time instance that represents the same time as this instance; otherwise, false.</returns>
	public override bool Equals( object obj )
	{
		if ( obj == null || GetType() != obj.GetType() )
		{
			return false;
		}

		return Equals( (Time)obj );
	} // Equals

	/// <summary>
	/// Returns the hash code for this instance.
	/// </summary>
	/// <returns>A 32-bit signed integer hash code.</returns>
	public override int GetHashCode()
	{
		return HashTool.ComputeHashCode( GetType().GetHashCode(), duration );
	} // GetHashCode

	/// <summary>
	/// Subtracts one Time from another and returns the resulting TimeSpan.
	/// </summary>
	/// <param name="time1">The Time to subtract from.</param>
	/// <param name="time2">The Time to subtract.</param>
	/// <returns>A TimeSpan representing the difference between the two times.</returns>
	public static TimeSpan operator -( Time time1, Time time2 )
	{
		return ( time1 - time2.duration ).duration;
	} // operator -

	/// <summary>
	/// Subtracts a TimeSpan from a Time and returns the resulting Time.
	/// </summary>
	/// <param name="time">The Time to subtract from.</param>
	/// <param name="duration">The TimeSpan to subtract.</param>
	/// <returns>A Time representing the result of the subtraction.</returns>
	public static Time operator -( Time time, TimeSpan duration )
	{
		if ( Equals( duration, TimeSpan.Zero ) )
		{
			return time;
		}
		DateTime day = duration > TimeSpan.Zero ? DateTime.MaxValue.Date : DateTime.MinValue.Date;
		return new Time( time.ToDateTime( day ).Subtract( duration ) );
	} // operator -

	/// <summary>
	/// Adds two Time instances and returns the resulting TimeSpan.
	/// </summary>
	/// <param name="time1">The first Time to add.</param>
	/// <param name="time2">The second Time to add.</param>
	/// <returns>A TimeSpan representing the sum of the two times.</returns>
	public static TimeSpan operator +( Time time1, Time time2 )
	{
		return ( time1 + time2.duration ).duration;
	} // operator +

	/// <summary>
	/// Adds a TimeSpan to a Time and returns the resulting Time.
	/// </summary>
	/// <param name="time">The Time to add to.</param>
	/// <param name="duration">The TimeSpan to add.</param>
	/// <returns>A Time representing the result of the addition.</returns>
	public static Time operator +( Time time, TimeSpan duration )
	{
		if ( Equals( duration, TimeSpan.Zero ) )
		{
			return time;
		}
		DateTime day = duration > TimeSpan.Zero ? DateTime.MinValue : DateTime.MaxValue;
		return new Time( time.ToDateTime( day ).Add( duration ) );
	} // operator +

	/// <summary>
	/// Determines whether one Time is less than another Time.
	/// </summary>
	/// <param name="time1">The first Time to compare.</param>
	/// <param name="time2">The second Time to compare.</param>
	/// <returns>true if time1 is less than time2; otherwise, false.</returns>
	public static bool operator <( Time time1, Time time2 )
	{
		return time1.duration < time2.duration;
	} // operator <

	/// <summary>
	/// Determines whether one Time is less than or equal to another Time.
	/// </summary>
	/// <param name="time1">The first Time to compare.</param>
	/// <param name="time2">The second Time to compare.</param>
	/// <returns>true if time1 is less than or equal to time2; otherwise, false.</returns>
	public static bool operator <=( Time time1, Time time2 )
	{
		return time1.duration <= time2.duration;
	} // operator <=

	/// <summary>
	/// Determines whether two Time instances are equal.
	/// </summary>
	/// <param name="left">The first Time to compare.</param>
	/// <param name="right">The second Time to compare.</param>
	/// <returns>true if the Time instances are equal; otherwise, false.</returns>
	public static bool operator ==( Time left, Time right )
	{
		return Equals( left, right );
	} // operator ==

	/// <summary>
	/// Determines whether two Time instances are not equal.
	/// </summary>
	/// <param name="left">The first Time to compare.</param>
	/// <param name="right">The second Time to compare.</param>
	/// <returns>true if the Time instances are not equal; otherwise, false.</returns>
	public static bool operator !=( Time left, Time right )
	{
		return !Equals( left, right );
	} // operator !=

	/// <summary>
	/// Determines whether one Time is greater than another Time.
	/// </summary>
	/// <param name="time1">The first Time to compare.</param>
	/// <param name="time2">The second Time to compare.</param>
	/// <returns>true if time1 is greater than time2; otherwise, false.</returns>
	public static bool operator >( Time time1, Time time2 )
	{
		return time1.duration > time2.duration;
	} // operator >

	/// <summary>
	/// Determines whether one Time is greater than or equal to another Time.
	/// </summary>
	/// <param name="time1">The first Time to compare.</param>
	/// <param name="time2">The second Time to compare.</param>
	/// <returns>true if time1 is greater than or equal to time2; otherwise, false.</returns>
	public static bool operator >=( Time time1, Time time2 )
	{
		return time1.duration >= time2.duration;
	} // operator >=

	/// <summary>
	/// Combines this time with the specified date to create a DateTime.
	/// </summary>
	/// <param name="date">The date to combine with this time.</param>
	/// <returns>A DateTime representing the combination of the date and this time.</returns>
	public DateTime ToDateTime( Date date )
	{
		return ToDateTime( date.DateTime );
	} // ToDateTime

	/// <summary>
	/// Combines this time with the date portion of the specified DateTime to create a new DateTime.
	/// </summary>
	/// <param name="dateTime">The DateTime whose date portion will be combined with this time.</param>
	/// <returns>A DateTime representing the combination of the date and this time.</returns>
	public DateTime ToDateTime( DateTime dateTime )
	{
		return ToDateTime( dateTime, this );
	} // ToDateTime

	/// <summary>
	/// Combines the specified date and time to create a DateTime.
	/// </summary>
	/// <param name="date">The date to combine.</param>
	/// <param name="time">The time to combine.</param>
	/// <returns>A DateTime representing the combination of the date and time.</returns>
	public static DateTime ToDateTime( Date date, Time time )
	{
		return ToDateTime( date.DateTime, time );
	} // ToDateTime

	/// <summary>
	/// Combines the date portion of the specified DateTime with the specified time to create a new DateTime.
	/// </summary>
	/// <param name="dateTime">The DateTime whose date portion will be used.</param>
	/// <param name="time">The time to combine with the date.</param>
	/// <returns>A DateTime representing the combination of the date and time.</returns>
	public static DateTime ToDateTime( DateTime dateTime, Time time )
	{
		return dateTime.Date.Add( time.Duration );
	} // ToDateTime

	// ----------------------------------------------------------------------
	// members
	private readonly TimeSpan duration;

} // struct Time

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
