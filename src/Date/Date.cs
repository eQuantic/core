using System;

namespace eQuantic.Core.Date;

/// <summary>
/// Represents a date without time information, providing date-only operations and comparisons.
/// </summary>
public struct Date : IComparable, IComparable<Date>, IEquatable<Date>
{

	/// <summary>
	/// Initializes a new instance of the Date struct from a DateTime value.
	/// </summary>
	/// <param name="date">The DateTime value to extract the date from.</param>
	public Date( DateTime date )
	{
		this.date = date.Date;
	} // Date

	/// <summary>
	/// Initializes a new instance of the Date struct with the specified year, month, and day.
	/// </summary>
	/// <param name="year">The year (must be between DateTime.MinValue.Year and DateTime.MaxValue.Year).</param>
	/// <param name="month">The month (1-12). Defaults to 1.</param>
	/// <param name="day">The day (1-31). Defaults to 1.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when year, month, or day is out of valid range.</exception>
	public Date( int year, int month = 1, int day = 1 )
	{
		if ( year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year )
		{
			throw new ArgumentOutOfRangeException( "year" );
		}
		if ( month <= 0 || month > TimeSpec.MonthsPerYear )
		{
			throw new ArgumentOutOfRangeException( "month" );
		}
		if ( day <= 0 || day > TimeSpec.MaxDaysPerMonth )
		{
			throw new ArgumentOutOfRangeException( "day" );
		}
		date = new DateTime( year, month, day );
	} // Date

	/// <summary>
	/// Gets the year component of the date.
	/// </summary>
	public int Year
	{
		get { return date.Year; }
	} // Year

	/// <summary>
	/// Gets the month component of the date.
	/// </summary>
	public int Month
	{
		get { return date.Month; }
	} // Month

	/// <summary>
	/// Gets the day component of the date.
	/// </summary>
	public int Day
	{
		get { return date.Day; }
	} // Day

	/// <summary>
	/// Gets the DateTime representation of this date.
	/// </summary>
	public DateTime DateTime
	{
		get { return date; }
	} // DateTime

	/// <summary>
	/// Compares this date with another date.
	/// </summary>
	/// <param name="other">The date to compare with.</param>
	/// <returns>A value less than zero if this date is earlier, zero if equal, or greater than zero if later.</returns>
	public int CompareTo( Date other )
	{
		return date.CompareTo( other.date );
	} // CompareTo

	/// <summary>
	/// Compares this date with another object.
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <returns>A value less than zero if this date is earlier, zero if equal, or greater than zero if later.</returns>
	public int CompareTo( object obj )
	{
		return date.CompareTo( ((Date)obj).date );
	} // CompareTo

	/// <summary>
	/// Determines whether this date is equal to another date.
	/// </summary>
	/// <param name="other">The date to compare with.</param>
	/// <returns>True if the dates are equal; otherwise, false.</returns>
	public bool Equals( Date other )
	{
		return date.Equals( other.date );
	} // Equals

	/// <summary>
	/// Returns a string representation of this date.
	/// </summary>
	/// <returns>A string representation of the date in short date format.</returns>
	public override string ToString()
	{
		return date.ToString( "d" ); // only the date part
	} // ToString

	/// <summary>
	/// Determines whether this date is equal to another object.
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <returns>True if the object is a Date and equal to this date; otherwise, false.</returns>
	public override bool Equals( object obj )
	{
		if ( obj == null || GetType() != obj.GetType() )
		{
			return false;
		}

		return Equals( (Date)obj );
	} // Equals

	/// <summary>
	/// Returns the hash code for this date.
	/// </summary>
	/// <returns>A hash code for this date.</returns>
	public override int GetHashCode()
	{
		return HashTool.ComputeHashCode( GetType().GetHashCode(), date );
	} // GetHashCode

	/// <summary>
	/// Calculates the time span between two dates.
	/// </summary>
	/// <param name="date1">The first date.</param>
	/// <param name="date2">The second date.</param>
	/// <returns>The time span between the two dates.</returns>
	public static TimeSpan operator -( Date date1, Date date2 )
	{
		return date1.date - date2.date;
	} // operator -

	/// <summary>
	/// Subtracts a time span from a date.
	/// </summary>
	/// <param name="date">The date to subtract from.</param>
	/// <param name="duration">The time span to subtract.</param>
	/// <returns>A new date that is the result of the subtraction.</returns>
	public static Date operator -( Date date, TimeSpan duration )
	{
		return new Date( date.date - duration );
	} // operator -

	/// <summary>
	/// Adds a time span to a date.
	/// </summary>
	/// <param name="date">The date to add to.</param>
	/// <param name="duration">The time span to add.</param>
	/// <returns>A new date that is the result of the addition.</returns>
	public static Date operator +( Date date, TimeSpan duration )
	{
		return new Date( date.date + duration );
	} // operator +

	/// <summary>
	/// Determines whether one date is less than another date.
	/// </summary>
	/// <param name="date1">The first date.</param>
	/// <param name="date2">The second date.</param>
	/// <returns>True if the first date is less than the second date; otherwise, false.</returns>
	public static bool operator <( Date date1, Date date2 )
	{
		return date1.date < date2.date;
	} // operator <

	/// <summary>
	/// Determines whether one date is less than or equal to another date.
	/// </summary>
	/// <param name="date1">The first date.</param>
	/// <param name="date2">The second date.</param>
	/// <returns>True if the first date is less than or equal to the second date; otherwise, false.</returns>
	public static bool operator <=( Date date1, Date date2 )
	{
		return date1.date <= date2.date;
	} // operator <=

	/// <summary>
	/// Determines whether two dates are equal.
	/// </summary>
	/// <param name="left">The first date.</param>
	/// <param name="right">The second date.</param>
	/// <returns>True if the dates are equal; otherwise, false.</returns>
	public static bool operator ==( Date left, Date right )
	{
		return Equals( left, right );
	} // operator ==

	/// <summary>
	/// Determines whether two dates are not equal.
	/// </summary>
	/// <param name="left">The first date.</param>
	/// <param name="right">The second date.</param>
	/// <returns>True if the dates are not equal; otherwise, false.</returns>
	public static bool operator !=( Date left, Date right )
	{
		return !Equals( left, right );
	} // operator !=

	/// <summary>
	/// Determines whether one date is greater than another date.
	/// </summary>
	/// <param name="date1">The first date.</param>
	/// <param name="date2">The second date.</param>
	/// <returns>True if the first date is greater than the second date; otherwise, false.</returns>
	public static bool operator >( Date date1, Date date2 )
	{
		return date1.date > date2.date;
	} // operator >

	/// <summary>
	/// Determines whether one date is greater than or equal to another date.
	/// </summary>
	/// <param name="date1">The first date.</param>
	/// <param name="date2">The second date.</param>
	/// <returns>True if the first date is greater than or equal to the second date; otherwise, false.</returns>
	public static bool operator >=( Date date1, Date date2 )
	{
		return date1.date >= date2.date;
	} // operator >=

	/// <summary>
	/// Converts this date to a DateTime by combining it with the specified time.
	/// </summary>
	/// <param name="time">The time to combine with this date.</param>
	/// <returns>A DateTime representing this date combined with the specified time.</returns>
	public DateTime ToDateTime( Time time )
	{
		return ToDateTime( this, time );
	} // ToDateTime

	/// <summary>
	/// Converts this date to a DateTime by combining it with the specified time components.
	/// </summary>
	/// <param name="hour">The hour component (0-23).</param>
	/// <param name="minute">The minute component (0-59). Defaults to 0.</param>
	/// <param name="second">The second component (0-59). Defaults to 0.</param>
	/// <param name="millisecond">The millisecond component (0-999). Defaults to 0.</param>
	/// <returns>A DateTime representing this date combined with the specified time components.</returns>
	public DateTime ToDateTime( int hour, int minute = 0, int second = 0, int millisecond = 0 )
	{
		return ToDateTime( this, hour, minute, second, millisecond );
	} // ToDateTime

	/// <summary>
	/// Converts a date to a DateTime by combining it with the specified time.
	/// </summary>
	/// <param name="date">The date to convert.</param>
	/// <param name="time">The time to combine with the date.</param>
	/// <returns>A DateTime representing the date combined with the specified time.</returns>
	public static DateTime ToDateTime( Date date, Time time )
	{
		return date.DateTime.Add( time.Duration );
	} // ToDateTime

	/// <summary>
	/// Converts a date to a DateTime by combining it with the specified time components.
	/// </summary>
	/// <param name="date">The date to convert.</param>
	/// <param name="hour">The hour component (0-23).</param>
	/// <param name="minute">The minute component (0-59). Defaults to 0.</param>
	/// <param name="second">The second component (0-59). Defaults to 0.</param>
	/// <param name="millisecond">The millisecond component (0-999). Defaults to 0.</param>
	/// <returns>A DateTime representing the date combined with the specified time components.</returns>
	public static DateTime ToDateTime( Date date, int hour, int minute = 0, int second = 0, int millisecond = 0 )
	{
		return new DateTime( date.Year, date.Month, date.Day, hour, minute, second, millisecond );
	} // ToDateTime

	// ----------------------------------------------------------------------
	// members
	private readonly DateTime date;

} // struct Date

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
