using System;
using System.Globalization;

namespace eQuantic.Core.Date;

/// <summary>
/// Provides utility methods for comparing DateTime values at different time granularities.
/// </summary>
public static class TimeCompare
{

	#region Year

	/// <summary>
	/// Determines whether two DateTime values are in the same calendar year.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same calendar year; otherwise, false.</returns>
	public static bool IsSameYear( DateTime left, DateTime right )
	{
		return left.Year == right.Year;
	} // IsSameYear

	/// <summary>
	/// Determines whether two DateTime values are in the same fiscal year based on the specified year start month.
	/// </summary>
	/// <param name="yearStartMonth">The month that starts the fiscal year.</param>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same fiscal year; otherwise, false.</returns>
	public static bool IsSameYear( YearMonth yearStartMonth, DateTime left, DateTime right )
	{
		return TimeTool.GetYearOf( yearStartMonth, left ) == TimeTool.GetYearOf( yearStartMonth, right );
	} // IsSameYear

	#endregion

	#region Hafyear

	/// <summary>
	/// Determines whether two DateTime values are in the same calendar half-year.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same calendar half-year; otherwise, false.</returns>
	public static bool IsSameHalfyear( DateTime left, DateTime right )
	{
		return IsSameHalfyear( TimeSpec.CalendarYearStartMonth, left, right );
	} // IsSameHalfyear

	/// <summary>
	/// Determines whether two DateTime values are in the same fiscal half-year based on the specified year start month.
	/// </summary>
	/// <param name="yearStartMonth">The month that starts the fiscal year.</param>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same fiscal half-year; otherwise, false.</returns>
	public static bool IsSameHalfyear( YearMonth yearStartMonth, DateTime left, DateTime right )
	{
		int leftYear = TimeTool.GetYearOf( yearStartMonth, left );
		int rightYear = TimeTool.GetYearOf( yearStartMonth, right );
		if ( leftYear != rightYear )
		{
			return false;
		}

		return TimeTool.GetHalfyearOfMonth( yearStartMonth, (YearMonth)left.Month ) == TimeTool.GetHalfyearOfMonth( yearStartMonth, (YearMonth)right.Month );
	} // IsSameHalfyear

	#endregion

	#region Quarter

	/// <summary>
	/// Determines whether two DateTime values are in the same calendar quarter.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same calendar quarter; otherwise, false.</returns>
	public static bool IsSameQuarter( DateTime left, DateTime right )
	{
		return IsSameQuarter( TimeSpec.CalendarYearStartMonth, left, right );
	} // IsSameQuarter

	/// <summary>
	/// Determines whether two DateTime values are in the same fiscal quarter based on the specified year start month.
	/// </summary>
	/// <param name="yearStartMonth">The month that starts the fiscal year.</param>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same fiscal quarter; otherwise, false.</returns>
	public static bool IsSameQuarter( YearMonth yearStartMonth, DateTime left, DateTime right )
	{
		int leftYear = TimeTool.GetYearOf( yearStartMonth, left );
		int rightYear = TimeTool.GetYearOf( yearStartMonth, right );
		if ( leftYear != rightYear )
		{
			return false;
		}

		return TimeTool.GetQuarterOfMonth( yearStartMonth, (YearMonth)left.Month ) == TimeTool.GetQuarterOfMonth( yearStartMonth, (YearMonth)right.Month );
	} // IsSameQuarter

	#endregion

	#region Month

	/// <summary>
	/// Determines whether two DateTime values are in the same month and year.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same month and year; otherwise, false.</returns>
	public static bool IsSameMonth( DateTime left, DateTime right )
	{
		return IsSameYear( left, right ) && left.Month == right.Month;
	} // IsSameMonth

	#endregion

	#region Week

	/// <summary>
	/// Determines whether two DateTime values are in the same week using the specified culture and week type.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <param name="culture">The culture to use for week calculations.</param>
	/// <param name="weekType">The type of week calculation to use.</param>
	/// <returns>True if both dates are in the same week; otherwise, false.</returns>
	public static bool IsSameWeek( DateTime left, DateTime right, CultureInfo culture, YearWeekType weekType )
	{
		return IsSameWeek( left, right, culture, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek, weekType );
	} // IsSameWeek

	/// <summary>
	/// Determines whether two DateTime values are in the same week using the specified culture, week rule, first day of week, and week type.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <param name="culture">The culture to use for week calculations.</param>
	/// <param name="weekRule">The calendar week rule to use.</param>
	/// <param name="firstDayOfWeek">The first day of the week.</param>
	/// <param name="weekType">The type of week calculation to use.</param>
	/// <returns>True if both dates are in the same week; otherwise, false.</returns>
	/// <exception cref="ArgumentNullException">Thrown when culture is null.</exception>
	public static bool IsSameWeek( DateTime left, DateTime right, CultureInfo culture, 
		CalendarWeekRule weekRule, DayOfWeek firstDayOfWeek, YearWeekType weekType )
	{
		if ( culture == null )
		{
			throw new ArgumentNullException( "culture" );
		}

		// left
		int leftWeekOfYear;
		int leftYear;
		TimeTool.GetWeekOfYear( left, culture, weekRule, firstDayOfWeek, weekType, out leftYear, out leftWeekOfYear );

		// rught
		int rightWeekOfYear;
		int rightYear;
		TimeTool.GetWeekOfYear( right, culture, weekRule, firstDayOfWeek, weekType, out rightYear, out rightWeekOfYear );

		return leftYear == rightYear && leftWeekOfYear == rightWeekOfYear;
	} // IsSameWeek

	#endregion

	#region Day

	/// <summary>
	/// Determines whether two DateTime values are on the same day.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are on the same day; otherwise, false.</returns>
	public static bool IsSameDay( DateTime left, DateTime right )
	{
		return IsSameMonth( left, right ) && left.Day == right.Day;
	} // IsSameDay

	#endregion

	#region Hour

	/// <summary>
	/// Determines whether two DateTime values are in the same hour.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same hour; otherwise, false.</returns>
	public static bool IsSameHour( DateTime left, DateTime right )
	{
		return IsSameDay( left, right ) && left.Hour == right.Hour;
	} // IsSameHour

	#endregion

	#region Minute

	/// <summary>
	/// Determines whether two DateTime values are in the same minute.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same minute; otherwise, false.</returns>
	public static bool IsSameMinute( DateTime left, DateTime right )
	{
		return IsSameHour( left, right ) && left.Minute == right.Minute;
	} // IsSameMinute

	#endregion

	#region Second

	/// <summary>
	/// Determines whether two DateTime values are in the same second.
	/// </summary>
	/// <param name="left">The first DateTime to compare.</param>
	/// <param name="right">The second DateTime to compare.</param>
	/// <returns>True if both dates are in the same second; otherwise, false.</returns>
	public static bool IsSameSecond( DateTime left, DateTime right )
	{
		return IsSameMinute( left, right ) && left.Second == right.Second;
	} // IsSameSecond

	#endregion

} // class TimeCompare

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
