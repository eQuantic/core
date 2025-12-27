using System;
using System.Globalization;
using System.Text;

namespace eQuantic.Core.Date;

/// <summary>
/// Provides formatting services for time periods, durations, and date/time values with culture-specific formatting.
/// </summary>
public class TimeFormatter : ITimeFormatter
{
	/// <summary>
	/// Initializes a new instance of the TimeFormatter class with the current culture.
	/// </summary>
	public TimeFormatter() :
		this( CultureInfo.CurrentCulture )
	{
	} // TimeFormatter

	/// <summary>
	/// Initializes a new instance of the TimeFormatter class with custom formatting options.
	/// </summary>
	/// <param name="culture">The culture to use for formatting. If null, uses the current culture.</param>
	/// <param name="contextSeparator">The separator used between context elements. Default is "; ".</param>
	/// <param name="startEndSeparator">The separator used between start and end elements. Default is " - ".</param>
	/// <param name="durationSeparator">The separator used for duration elements. Default is " | ".</param>
	/// <param name="dateTimeFormat">The format string for date and time. If null, uses culture default.</param>
	/// <param name="shortDateFormat">The format string for short dates. If null, uses culture default.</param>
	/// <param name="longTimeFormat">The format string for long time. If null, uses culture default.</param>
	/// <param name="shortTimeFormat">The format string for short time. If null, uses culture default.</param>
	/// <param name="durationType">The type of duration formatting to use. Default is Compact.</param>
	/// <param name="useDurationSeconds">Whether to include seconds in duration formatting. Default is false.</param>
	/// <param name="useIsoIntervalNotation">Whether to use ISO interval notation. Default is false.</param>
	/// <param name="durationItemSeparator">The separator between duration items. Default is " ".</param>
	/// <param name="durationLastItemSeparator">The separator before the last duration item. Default is " ".</param>
	/// <param name="durationValueSeparator">The separator between duration values and units. Default is " ".</param>
	/// <param name="intervalStartClosed">The string for closed interval start. Default is "[".</param>
	/// <param name="intervalStartOpen">The string for open interval start. Default is "(".</param>
	/// <param name="intervalStartOpenIso">The string for open interval start in ISO notation. Default is "]".</param>
	/// <param name="intervalEndClosed">The string for closed interval end. Default is "]".</param>
	/// <param name="intervalEndOpen">The string for open interval end. Default is ")".</param>
	/// <param name="intervalEndOpenIso">The string for open interval end in ISO notation. Default is "[".</param>
	public TimeFormatter( CultureInfo culture = null,
		string contextSeparator = "; ", string startEndSeparator = " - ",
		string durationSeparator = " | ",
		string dateTimeFormat = null,
		string shortDateFormat = null,
		string longTimeFormat = null,
		string shortTimeFormat = null,
		DurationFormatType durationType = DurationFormatType.Compact,
		bool useDurationSeconds = false,
		bool useIsoIntervalNotation = false,
		string durationItemSeparator = " ",
		string durationLastItemSeparator = " ",
		string durationValueSeparator = " ",
		string intervalStartClosed = "[",
		string intervalStartOpen = "(",
		string intervalStartOpenIso = "]",
		string intervalEndClosed = "]",
		string intervalEndOpen = ")",
		string intervalEndOpenIso = "[" )
	{
		if ( culture == null )
		{
			culture = CultureInfo.CurrentCulture;
		}
		this.culture = culture;
		listSeparator = culture.TextInfo.ListSeparator;
		this.contextSeparator = contextSeparator;
		this.startEndSeparator = startEndSeparator;
		this.durationSeparator = durationSeparator;
		this.durationItemSeparator = durationItemSeparator;
		this.durationLastItemSeparator = durationLastItemSeparator;
		this.durationValueSeparator = durationValueSeparator;
		this.intervalStartClosed = intervalStartClosed;
		this.intervalStartOpen = intervalStartOpen;
		this.intervalStartOpenIso = intervalStartOpenIso;
		this.intervalEndClosed = intervalEndClosed;
		this.intervalEndOpen = intervalEndOpen;
		this.intervalEndOpenIso = intervalEndOpenIso;
		this.dateTimeFormat = dateTimeFormat;
		this.shortDateFormat = shortDateFormat;
		this.longTimeFormat = longTimeFormat;
		this.shortTimeFormat = shortTimeFormat;
		this.durationType = durationType;
		this.useDurationSeconds = useDurationSeconds;
		this.useIsoIntervalNotation = useIsoIntervalNotation;
	} // TimeFormatter

	/// <summary>
	/// Gets or sets the singleton instance of the TimeFormatter class.
	/// </summary>
	/// <value>The default TimeFormatter instance.</value>
	/// <exception cref="ArgumentNullException">Thrown when attempting to set the value to null.</exception>
	public static TimeFormatter Instance
	{
		get
		{
			if ( instance == null )
			{
				lock ( mutex )
				{
					if ( instance == null )
					{
						instance = new TimeFormatter();
					}
				}
			}
			return instance;
		}
		set
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}
			lock ( mutex )
			{
				instance = value;
			}
		}
	} // Instance

	/// <summary>
	/// Gets the culture used for formatting operations.
	/// </summary>
	public CultureInfo Culture
	{
		get { return culture; }
	} // Culture

	/// <summary>
	/// Gets the separator used for list items.
	/// </summary>
	public string ListSeparator
	{
		get { return listSeparator; }
	} // ListSeparator

	/// <summary>
	/// Gets the separator used between context elements.
	/// </summary>
	public string ContextSeparator
	{
		get { return contextSeparator; }
	} // ContextSeparator

	/// <summary>
	/// Gets the separator used between start and end elements.
	/// </summary>
	public string StartEndSeparator
	{
		get { return startEndSeparator; }
	} // StartEndSeparator

	/// <summary>
	/// Gets the separator used for duration elements.
	/// </summary>
	public string DurationSeparator
	{
		get { return durationSeparator; }
	} // DurationSeparator

	/// <summary>
	/// Gets the separator used between duration items.
	/// </summary>
	public string DurationItemSeparator
	{
		get { return durationItemSeparator; }
	} // DurationItemSeparator

	/// <summary>
	/// Gets the separator used before the last duration item.
	/// </summary>
	public string DurationLastItemSeparator
	{
		get { return durationLastItemSeparator; }
	} // DurationLastItemSeparator

	/// <summary>
	/// Gets the separator used between duration values and their units.
	/// </summary>
	public string DurationValueSeparator
	{
		get { return durationValueSeparator; }
	} // DurationValueSeparator

	/// <summary>
	/// Gets the string used to represent a closed interval start.
	/// </summary>
	public string IntervalStartClosed
	{
		get { return intervalStartClosed; }
	} // IntervalStartClosed

	/// <summary>
	/// Gets the string used to represent an open interval start.
	/// </summary>
	public string IntervalStartOpen
	{
		get { return intervalStartOpen; }
	} // IntervalStartOpen

	/// <summary>
	/// Gets the string used to represent an open interval start in ISO notation.
	/// </summary>
	public string IntervalStartOpenIso
	{
		get { return intervalStartOpenIso; }
	} // IntervalStartOpenIso

	/// <summary>
	/// Gets the string used to represent a closed interval end.
	/// </summary>
	public string IntervalEndClosed
	{
		get { return intervalEndClosed; }
	} // IntervalEndClosed

	/// <summary>
	/// Gets the string used to represent an open interval end.
	/// </summary>
	public string IntervalEndOpen
	{
		get { return intervalEndOpen; }
	} // IntervalEndOpen

	/// <summary>
	/// Gets the string used to represent an open interval end in ISO notation.
	/// </summary>
	public string IntervalEndOpenIso
	{
		get { return intervalEndOpenIso; }
	} // IntervalEndOpenIso

	/// <summary>
	/// Gets the format string used for date and time formatting.
	/// </summary>
	public string DateTimeFormat
	{
		get { return dateTimeFormat; }
	} // DateTimeFormat

	/// <summary>
	/// Gets the format string used for short date formatting.
	/// </summary>
	public string ShortDateFormat
	{
		get { return shortDateFormat; }
	} // ShortDateFormat

	/// <summary>
	/// Gets the format string used for long time formatting.
	/// </summary>
	public string LongTimeFormat
	{
		get { return longTimeFormat; }
	} // LongTimeFormat

	/// <summary>
	/// Gets the format string used for short time formatting.
	/// </summary>
	public string ShortTimeFormat
	{
		get { return shortTimeFormat; }
	} // ShortTimeFormat

	/// <summary>
	/// Gets the duration format type to use for formatting durations.
	/// </summary>
	public DurationFormatType DurationType
	{
		get { return durationType; }
	} // DurationType

	/// <summary>
	/// Gets a value indicating whether to include seconds in duration formatting.
	/// </summary>
	public bool UseDurationSeconds
	{
		get { return useDurationSeconds; }
	} // UseDurationSeconds

	/// <summary>
	/// Gets a value indicating whether to use ISO interval notation for formatting intervals.
	/// </summary>
	public bool UseIsoIntervalNotation
	{
		get { return useIsoIntervalNotation; }
	} // UseIsoIntervalNotation

	#region Collection

	/// <summary>
	/// Gets a formatted string representation of a collection count.
	/// </summary>
	/// <param name="count">The number of items in the collection.</param>
	/// <returns>A formatted string representing the collection count.</returns>
	public virtual string GetCollection( int count )
	{
		return string.Format( "Count = {0}", count );
	} // GetCollection

	/// <summary>
	/// Gets a formatted string representation of a collection period.
	/// </summary>
	/// <param name="count">The number of items in the collection.</param>
	/// <param name="start">The start date of the period.</param>
	/// <param name="end">The end date of the period.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted string representing the collection period.</returns>
	public virtual string GetCollectionPeriod( int count, DateTime start, DateTime end, TimeSpan duration )
	{
		return string.Format( "{0}{1} {2}", GetCollection( count ), ListSeparator, GetPeriod( start, end, duration ) );
	} // GetCollectionPeriod

	#endregion

	#region DateTime

	/// <summary>
	/// Gets a formatted string representation of a date and time.
	/// </summary>
	/// <param name="dateTime">The date and time to format.</param>
	/// <returns>A formatted date and time string.</returns>
	public string GetDateTime( DateTime dateTime )
	{
		return !string.IsNullOrEmpty( dateTimeFormat ) ? dateTime.ToString( dateTimeFormat ) : dateTime.ToString( culture );
	} // GetDateTime

	/// <summary>
	/// Gets a formatted string representation of a date in short format.
	/// </summary>
	/// <param name="dateTime">The date to format.</param>
	/// <returns>A formatted short date string.</returns>
	public string GetShortDate( DateTime dateTime )
	{
		return !string.IsNullOrEmpty( shortDateFormat ) ? dateTime.ToString( shortDateFormat ) : dateTime.ToString( "d" );
	} // GetShortDate

	/// <summary>
	/// Gets a formatted string representation of a time in long format.
	/// </summary>
	/// <param name="dateTime">The time to format.</param>
	/// <returns>A formatted long time string.</returns>
	public string GetLongTime( DateTime dateTime )
	{
		return !string.IsNullOrEmpty( longTimeFormat ) ? dateTime.ToString( longTimeFormat ) : dateTime.ToString( "T" );
	} // GetLongTime

	/// <summary>
	/// Gets a formatted string representation of a time in short format.
	/// </summary>
	/// <param name="dateTime">The time to format.</param>
	/// <returns>A formatted short time string.</returns>
	public string GetShortTime( DateTime dateTime )
	{
		return !string.IsNullOrEmpty( shortTimeFormat ) ? dateTime.ToString( shortTimeFormat ) : dateTime.ToString( "t" );
	} // GetShortTime

	#endregion

	#region Duration

	/// <summary>
	/// Gets a formatted string representation of a time period.
	/// </summary>
	/// <param name="start">The start date of the period.</param>
	/// <param name="end">The end date of the period.</param>
	/// <returns>A formatted period string.</returns>
	public string GetPeriod( DateTime start, DateTime end )
	{
		return GetPeriod( start, end, end - start );
	} // GetPeriod

	/// <summary>
	/// Gets a formatted string representation of a duration.
	/// </summary>
	/// <param name="timeSpan">The duration to format.</param>
	/// <returns>A formatted duration string.</returns>
	public string GetDuration( TimeSpan timeSpan )
	{
		return GetDuration( timeSpan, durationType );
	} // GetDuration

	/// <summary>
	/// Gets a formatted string representation of a duration using the specified format type.
	/// </summary>
	/// <param name="timeSpan">The duration to format.</param>
	/// <param name="durationFormatType">The format type to use.</param>
	/// <returns>A formatted duration string.</returns>
	public string GetDuration( TimeSpan timeSpan, DurationFormatType durationFormatType )
	{
		switch ( durationFormatType )
		{
			case DurationFormatType.Detailed:
				int days = (int)timeSpan.TotalDays;
				int hours = timeSpan.Hours;
				int minutes = timeSpan.Minutes;
				int seconds = UseDurationSeconds ? timeSpan.Seconds : 0;
				return GetDuration( 0, 0, days, hours, minutes, seconds );
			default:
				return UseDurationSeconds ?
					string.Format( "{0}.{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds ) :
					string.Format( "{0}.{1:00}:{2:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes );
		}
	} // GetDuration

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
	public virtual string GetDuration( int years, int months, int days, int hours, int minutes, int seconds )
	{
		StringBuilder sb = new StringBuilder();

		// years(s)
		if ( years != 0 )
		{
			sb.Append( years );
			sb.Append( DurationValueSeparator );
			sb.Append( years == 1 ? Strings.TimeSpanYear : Strings.TimeSpanYears );
		}

		// month(s)
		if ( months != 0 )
		{
			if ( sb.Length > 0 )
			{
				sb.Append( days == 0 && hours == 0 && minutes == 0 && seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator );
			}
			sb.Append( months );
			sb.Append( DurationValueSeparator );
			sb.Append( months == 1 ? Strings.TimeSpanMonth : Strings.TimeSpanMonths );
		}

		// day(s)
		if ( days != 0 )
		{
			if ( sb.Length > 0 )
			{
				sb.Append( hours == 0 && minutes == 0 && seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator );
			}
			sb.Append( days );
			sb.Append( DurationValueSeparator );
			sb.Append( days == 1 ? Strings.TimeSpanDay : Strings.TimeSpanDays );
		}

		// hour(s)
		if ( hours != 0 )
		{
			if ( sb.Length > 0 )
			{
				sb.Append( minutes == 0 && seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator );
			}
			sb.Append( hours );
			sb.Append( DurationValueSeparator );
			sb.Append( hours == 1 ? Strings.TimeSpanHour : Strings.TimeSpanHours );
		}

		// minute(s)
		if ( minutes != 0 )
		{
			if ( sb.Length > 0 )
			{
				sb.Append( seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator );
			}
			sb.Append( minutes );
			sb.Append( DurationValueSeparator );
			sb.Append( minutes == 1 ? Strings.TimeSpanMinute : Strings.TimeSpanMinutes );
		}

		// second(s)
		if ( seconds != 0 )
		{
			if ( sb.Length > 0 )
			{
				sb.Append( DurationLastItemSeparator );
			}
			sb.Append( seconds );
			sb.Append( DurationValueSeparator );
			sb.Append( seconds == 1 ? Strings.TimeSpanSecond : Strings.TimeSpanSeconds );
		}

		return sb.ToString();
	} // GetDuration

	#endregion

	#region Period

	/// <summary>
	/// Gets a formatted string representation of a time period with duration.
	/// </summary>
	/// <param name="start">The start date of the period.</param>
	/// <param name="end">The end date of the period.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted period string with duration.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when end is less than start.</exception>
	public virtual string GetPeriod( DateTime start, DateTime end, TimeSpan duration )
	{
		if ( end < start )
		{
			throw new ArgumentOutOfRangeException( "end" );
		}

		bool startHasTimeOfDay = TimeTool.HasTimeOfDay( start );

		// no duration - schow start date (optionally with the time part)
		if ( duration == TimeSpec.MinPeriodDuration )
		{
			return startHasTimeOfDay ? GetDateTime( start ) : GetShortDate( start );
		}

		// within one day: show full start, end time and suration
		if ( TimeCompare.IsSameDay( start, end ) )
		{
			return GetDateTime( start ) + startEndSeparator + GetLongTime( end ) + durationSeparator + GetDuration( duration );
		}

		// show start date, end date and duration (optionally with the time part)
		bool endHasTimeOfDay = TimeTool.HasTimeOfDay( start );
		bool hasTimeOfDays = startHasTimeOfDay || endHasTimeOfDay;
		string startPart = hasTimeOfDays ? GetDateTime( start ) : GetShortDate( start );
		string endPart = hasTimeOfDays ? GetDateTime( end ) : GetShortDate( end );
		return startPart + startEndSeparator + endPart + durationSeparator + GetDuration( duration );
	} // GetPeriod

	/// <summary>
	/// Gets a formatted string representation of a calendar period.
	/// </summary>
	/// <param name="start">The start description.</param>
	/// <param name="end">The end description.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted calendar period string.</returns>
	public string GetCalendarPeriod( string start, string end, TimeSpan duration )
	{
		string timePeriod = start.Equals( end ) ? start : start + startEndSeparator + end;
		return timePeriod + durationSeparator + GetDuration( duration );
	} // GetCalendarPeriod

	/// <summary>
	/// Gets a formatted string representation of a calendar period with context.
	/// </summary>
	/// <param name="context">The context description.</param>
	/// <param name="start">The start description.</param>
	/// <param name="end">The end description.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted calendar period string with context.</returns>
	public string GetCalendarPeriod( string context, string start, string end, TimeSpan duration )
	{
		string timePeriod = start.Equals( end ) ? start : start + startEndSeparator + end;
		return context + contextSeparator + timePeriod + durationSeparator + GetDuration( duration );
	} // GetCalendarPeriod

	/// <summary>
	/// Gets a formatted string representation of a calendar period with separate start and end contexts.
	/// </summary>
	/// <param name="startContext">The start context description.</param>
	/// <param name="endContext">The end context description.</param>
	/// <param name="start">The start description.</param>
	/// <param name="end">The end description.</param>
	/// <param name="duration">The duration of the period.</param>
	/// <returns>A formatted calendar period string with contexts.</returns>
	public string GetCalendarPeriod( string startContext, string endContext, string start, string end, TimeSpan duration )
	{
		string contextPeriod = startContext.Equals( endContext ) ? startContext : startContext + startEndSeparator + endContext;
		string timePeriod = start.Equals( end ) ? start : start + startEndSeparator + end;
		return contextPeriod + contextSeparator + timePeriod + durationSeparator + GetDuration( duration );
	} // GetCalendarPeriod

	#endregion

	#region Interval

	/// <summary>
	/// Gets a formatted string representation of an interval with specified edge types.
	/// </summary>
	/// <param name="start">The start date of the interval.</param>
	/// <param name="end">The end date of the interval.</param>
	/// <param name="startEdge">The type of the start edge (open or closed).</param>
	/// <param name="endEdge">The type of the end edge (open or closed).</param>
	/// <param name="duration">The duration of the interval.</param>
	/// <returns>A formatted interval string.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when end is less than start.</exception>
	public string GetInterval( DateTime start, DateTime end,
		IntervalEdge startEdge, IntervalEdge endEdge, TimeSpan duration )
	{
		if ( end < start )
		{
			throw new ArgumentOutOfRangeException( "end" );
		}

		StringBuilder sb = new StringBuilder();

		// interval start
		switch ( startEdge )
		{
			case IntervalEdge.Closed:
				sb.Append( IntervalStartClosed );
				break;
			case IntervalEdge.Open:
				sb.Append( UseIsoIntervalNotation ? intervalStartOpenIso : intervalStartOpen );
				break;
		}

		bool addDuration = true;
		bool startHasTimeOfDay = TimeTool.HasTimeOfDay( start );

		// no duration - schow start date (optionally with the time part)
		if ( duration == TimeSpec.MinPeriodDuration )
		{
			sb.Append( startHasTimeOfDay ? GetDateTime( start ) : GetShortDate( start ) );
			addDuration = false;
		}
		// within one day: show full start, end time and suration
		else if ( TimeCompare.IsSameDay( start, end ) )
		{
			sb.Append( GetDateTime( start ) );
			sb.Append( startEndSeparator );
			sb.Append( GetLongTime( end ) );
		}
		else
		{
			bool endHasTimeOfDay = TimeTool.HasTimeOfDay( start );
			bool hasTimeOfDays = startHasTimeOfDay || endHasTimeOfDay;
			if ( hasTimeOfDays )
			{
				sb.Append( GetDateTime( start ) );
				sb.Append( startEndSeparator );
				sb.Append( GetDateTime( end ) );
			}
			else
			{
				sb.Append( GetShortDate( start ) );
				sb.Append( startEndSeparator );
				sb.Append( GetShortDate( end ) );
			}
		}

		// interval end
		switch ( endEdge )
		{
			case IntervalEdge.Closed:
				sb.Append( IntervalEndClosed );
				break;
			case IntervalEdge.Open:
				sb.Append( UseIsoIntervalNotation ? IntervalEndOpenIso : IntervalEndOpen );
				break;
		}

		// duration
		if ( addDuration )
		{
			sb.Append( durationSeparator );
			sb.Append( GetDuration( duration ) );
		}

		return sb.ToString();
	} // GetInterval

	#endregion

	// ----------------------------------------------------------------------
	// members
	private readonly CultureInfo culture;
	private readonly string listSeparator;
	private readonly string contextSeparator;
	private readonly string startEndSeparator;
	private readonly string durationSeparator;
	private readonly string durationItemSeparator;
	private readonly string durationLastItemSeparator;
	private readonly string durationValueSeparator;
	private readonly string intervalStartClosed;
	private readonly string intervalStartOpen;
	private readonly string intervalStartOpenIso;
	private readonly string intervalEndClosed;
	private readonly string intervalEndOpen;
	private readonly string intervalEndOpenIso;
	private readonly string dateTimeFormat;
	private readonly string shortDateFormat;
	private readonly string longTimeFormat;
	private readonly string shortTimeFormat;
	private readonly DurationFormatType durationType;
	private readonly bool useDurationSeconds;
	private readonly bool useIsoIntervalNotation;

	private static readonly object mutex = new object();
	private static volatile TimeFormatter instance;

} // class TimeFormatter

// namespace Itenso.TimePeriod
// -- EOF -------------------------------------------------------------------
