using System;
using System.Globalization;

namespace eQuantic.Core.Date;

/// <summary>
/// Defines a contract for formatting time periods, durations, and date/time values.
/// </summary>
public interface ITimeFormatter
{
	/// <summary>
	/// Gets the culture used for formatting.
	/// </summary>
	CultureInfo Culture { get; }

	/// <summary>
	/// Gets the separator used for list items.
	/// </summary>
	string ListSeparator { get; }

	/// <summary>
	/// Gets the separator used between context elements.
	/// </summary>
	string ContextSeparator { get; }

	/// <summary>
	/// Gets the separator used between start and end elements.
	/// </summary>
	string StartEndSeparator { get; }

	/// <summary>
	/// Gets the separator used for duration elements.
	/// </summary>
	string DurationSeparator { get; }

	/// <summary>
	/// Gets the separator used between duration items.
	/// </summary>
	string DurationItemSeparator { get; }

	/// <summary>
	/// Gets the separator used before the last duration item.
	/// </summary>
	string DurationLastItemSeparator { get; }

	/// <summary>
	/// Gets the separator used between duration values and their units.
	/// </summary>
	string DurationValueSeparator { get; }

	/// <summary>
	/// Gets the string used to represent a closed interval start.
	/// </summary>
	string IntervalStartClosed { get; }

	/// <summary>
	/// Gets the string used to represent an open interval start.
	/// </summary>
	string IntervalStartOpen { get; }

	/// <summary>
	/// Gets the string used to represent an open interval start in ISO notation.
	/// </summary>
	string IntervalStartOpenIso { get; }

	/// <summary>
	/// Gets the string used to represent a closed interval end.
	/// </summary>
	string IntervalEndClosed { get; }

	/// <summary>
	/// Gets the string used to represent an open interval end.
	/// </summary>
	string IntervalEndOpen { get; }

	/// <summary>
	/// Gets the string used to represent an open interval end in ISO notation.
	/// </summary>
	string IntervalEndOpenIso { get; }

	/// <summary>
	/// Gets the format string used for date and time formatting.
	/// </summary>
	string DateTimeFormat { get; }

	/// <summary>
	/// Gets the format string used for short date formatting.
	/// </summary>
	string ShortDateFormat { get; }

	/// <summary>
	/// Gets the format string used for long time formatting.
	/// </summary>
	string LongTimeFormat { get; }

	/// <summary>
	/// Gets the format string used for short time formatting.
	/// </summary>
	string ShortTimeFormat { get; }

	/// <summary>
	/// Gets the duration format type to use for formatting durations.
	/// </summary>
	DurationFormatType DurationType { get; }

	/// <summary>
	/// Gets a value indicating whether to include seconds in duration formatting.
	/// </summary>
	bool UseDurationSeconds { get; }

	/// <summary>
	/// Gets a formatted string representation of a collection count.
	/// </summary>
	/// <param name="count">The number of items in the collection.</param>
	/// <returns>A formatted string representing the collection count.</returns>
	string GetCollection( int count );

	/// <summary>
	/// Gets a formatted string representation of a collection period.
	/// </summary>
	/// <param name="count">The number of items in the collection.</param>
	/// <param name="start">The start date of the period.</param>
	/// <param name="end">The end date of the period.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted string representing the collection period.</returns>
	string GetCollectionPeriod( int count, DateTime start, DateTime end, TimeSpan duration );

	/// <summary>
	/// Gets a formatted string representation of a date and time.
	/// </summary>
	/// <param name="dateTime">The date and time to format.</param>
	/// <returns>A formatted date and time string.</returns>
	string GetDateTime( DateTime dateTime );

	/// <summary>
	/// Gets a formatted string representation of a date in short format.
	/// </summary>
	/// <param name="dateTime">The date to format.</param>
	/// <returns>A formatted short date string.</returns>
	string GetShortDate( DateTime dateTime );

	/// <summary>
	/// Gets a formatted string representation of a time in long format.
	/// </summary>
	/// <param name="dateTime">The time to format.</param>
	/// <returns>A formatted long time string.</returns>
	string GetLongTime( DateTime dateTime );

	/// <summary>
	/// Gets a formatted string representation of a time in short format.
	/// </summary>
	/// <param name="dateTime">The time to format.</param>
	/// <returns>A formatted short time string.</returns>
	string GetShortTime( DateTime dateTime );

	/// <summary>
	/// Gets a formatted string representation of a time period.
	/// </summary>
	/// <param name="start">The start date of the period.</param>
	/// <param name="end">The end date of the period.</param>
	/// <returns>A formatted period string.</returns>
	string GetPeriod( DateTime start, DateTime end );

	/// <summary>
	/// Gets a formatted string representation of a duration.
	/// </summary>
	/// <param name="timeSpan">The duration to format.</param>
	/// <returns>A formatted duration string.</returns>
	string GetDuration( TimeSpan timeSpan );

	/// <summary>
	/// Gets a formatted string representation of a duration using the specified format type.
	/// </summary>
	/// <param name="timeSpan">The duration to format.</param>
	/// <param name="durationFormatType">The format type to use.</param>
	/// <returns>A formatted duration string.</returns>
	string GetDuration( TimeSpan timeSpan, DurationFormatType durationFormatType );

	/// <summary>
	/// Gets a formatted string representation of a duration from individual time components.
	/// </summary>
	/// <param name="years">The number of years.</param>
	/// <param name="months">The number of months.</param>
	/// <param name="days">The number of days.</param>
	/// <param name="hours">The number of hours.</param>
	/// <param name="minutes">The number of minutes.</param>
	/// <param name="seconds">The number of seconds.</param>
	/// <returns>A formatted duration string.</returns>
	string GetDuration( int years, int months, int days, int hours, int minutes, int seconds );

	/// <summary>
	/// Gets a formatted string representation of a time period with duration.
	/// </summary>
	/// <param name="start">The start date of the period.</param>
	/// <param name="end">The end date of the period.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted period string with duration.</returns>
	string GetPeriod( DateTime start, DateTime end, TimeSpan duration );

	/// <summary>
	/// Gets a formatted string representation of an interval with specified edge types.
	/// </summary>
	/// <param name="start">The start date of the interval.</param>
	/// <param name="end">The end date of the interval.</param>
	/// <param name="startEdge">The type of the start edge (open or closed).</param>
	/// <param name="endEdge">The type of the end edge (open or closed).</param>
	/// <param name="duration">The duration of the interval.</param>
	/// <returns>A formatted interval string.</returns>
	string GetInterval( DateTime start, DateTime end, IntervalEdge startEdge, IntervalEdge endEdge, TimeSpan duration );

	/// <summary>
	/// Gets a formatted string representation of a calendar period.
	/// </summary>
	/// <param name="start">The start description.</param>
	/// <param name="end">The end description.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted calendar period string.</returns>
	string GetCalendarPeriod( string start, string end, TimeSpan duration );

	/// <summary>
	/// Gets a formatted string representation of a calendar period with context.
	/// </summary>
	/// <param name="context">The context description.</param>
	/// <param name="start">The start description.</param>
	/// <param name="end">The end description.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted calendar period string with context.</returns>
	string GetCalendarPeriod( string context, string start, string end, TimeSpan duration );

	/// <summary>
	/// Gets a formatted string representation of a calendar period with separate start and end contexts.
	/// </summary>
	/// <param name="startContext">The start context description.</param>
	/// <param name="endContext">The end context description.</param>
	/// <param name="start">The start description.</param>
	/// <param name="end">The end description.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted calendar period string with contexts.</returns>
	string GetCalendarPeriod( string startContext, string endContext, string start, string end, TimeSpan duration );

} // interface ITimeFormatter

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
