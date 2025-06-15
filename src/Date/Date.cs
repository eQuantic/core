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

	// ----------------------------------------------------------------------
	public DateTime DateTime
	{
		get { return date; }
	} // DateTime

	// ----------------------------------------------------------------------
	public int CompareTo( Date other )
	{
		return date.CompareTo( other.date );
	} // CompareTo

	// ----------------------------------------------------------------------
	public int CompareTo( object obj )
	{
		return date.CompareTo( ((Date)obj).date );
	} // CompareTo

	// ----------------------------------------------------------------------
	public bool Equals( Date other )
	{
		return date.Equals( other.date );
	} // Equals

	// ----------------------------------------------------------------------
	public override string ToString()
	{
		return date.ToString( "d" ); // only the date part
	} // ToString

	// ----------------------------------------------------------------------
	public override bool Equals( object obj )
	{
		if ( obj == null || GetType() != obj.GetType() )
		{
			return false;
		}

		return Equals( (Date)obj );
	} // Equals

	// ----------------------------------------------------------------------
	public override int GetHashCode()
	{
		return HashTool.ComputeHashCode( GetType().GetHashCode(), date );
	} // GetHashCode

	// ----------------------------------------------------------------------
	public static TimeSpan operator -( Date date1, Date date2 )
	{
		return date1.date - date2.date;
	} // operator -

	// ----------------------------------------------------------------------
	public static Date operator -( Date date, TimeSpan duration )
	{
		return new Date( date.date - duration );
	} // operator -

	// ----------------------------------------------------------------------
	public static Date operator +( Date date, TimeSpan duration )
	{
		return new Date( date.date + duration );
	} // operator +

	// ----------------------------------------------------------------------
	public static bool operator <( Date date1, Date date2 )
	{
		return date1.date < date2.date;
	} // operator <

	// ----------------------------------------------------------------------
	public static bool operator <=( Date date1, Date date2 )
	{
		return date1.date <= date2.date;
	} // operator <=

	// ----------------------------------------------------------------------
	public static bool operator ==( Date left, Date right )
	{
		return Equals( left, right );
	} // operator ==

	// ----------------------------------------------------------------------
	public static bool operator !=( Date left, Date right )
	{
		return !Equals( left, right );
	} // operator !=

	// ----------------------------------------------------------------------
	public static bool operator >( Date date1, Date date2 )
	{
		return date1.date > date2.date;
	} // operator >

	// ----------------------------------------------------------------------
	public static bool operator >=( Date date1, Date date2 )
	{
		return date1.date >= date2.date;
	} // operator >=

	// ----------------------------------------------------------------------
	public DateTime ToDateTime( Time time )
	{
		return ToDateTime( this, time );
	} // ToDateTime

	// ----------------------------------------------------------------------
	public DateTime ToDateTime( int hour, int minute = 0, int second = 0, int millisecond = 0 )
	{
		return ToDateTime( this, hour, minute, second, millisecond );
	} // ToDateTime

	// ----------------------------------------------------------------------
	public static DateTime ToDateTime( Date date, Time time )
	{
		return date.DateTime.Add( time.Duration );
	} // ToDateTime

	// ----------------------------------------------------------------------
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
