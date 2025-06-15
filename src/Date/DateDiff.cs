using System;
using System.Globalization;

namespace eQuantic.Core.Date;

/// <summary>
/// Represents a date difference calculator that provides various time unit calculations between two dates.
/// This class calculates differences in years, quarters, months, weeks, days, hours, minutes, and seconds.
/// </summary>
public sealed class DateDiff
{

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with a single date.
	/// The difference is calculated from the specified date to the current time.
	/// </summary>
	/// <param name="date">The date to calculate difference from.</param>
	public DateDiff( DateTime date ) :
		this( date, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek )
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with a single date and calendar settings.
	/// The difference is calculated from the specified date to the current time using the provided calendar settings.
	/// </summary>
	/// <param name="date">The date to calculate difference from.</param>
	/// <param name="calendar">The calendar to use for calculations.</param>
	/// <param name="firstDayOfWeek">The first day of the week for week calculations.</param>
	/// <param name="yearBaseMonth">The base month for year calculations.</param>
	public DateDiff( DateTime date, Calendar calendar, DayOfWeek firstDayOfWeek,
		YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth ) :
		this( date, ClockProxy.Clock.Now, calendar, firstDayOfWeek, yearBaseMonth )
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with two dates.
	/// Uses the current culture's calendar and first day of week settings.
	/// </summary>
	/// <param name="date1">The first date (start date).</param>
	/// <param name="date2">The second date (end date).</param>
	public DateDiff( DateTime date1, DateTime date2 ) :
		this( date1, date2, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek )
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with two dates and calendar settings.
	/// </summary>
	/// <param name="date1">The first date (start date).</param>
	/// <param name="date2">The second date (end date).</param>
	/// <param name="calendar">The calendar to use for calculations.</param>
	/// <param name="firstDayOfWeek">The first day of the week for week calculations.</param>
	/// <param name="yearBaseMonth">The base month for year calculations.</param>
	/// <exception cref="ArgumentNullException">Thrown when calendar is null.</exception>
	public DateDiff( DateTime date1, DateTime date2, Calendar calendar,
		DayOfWeek firstDayOfWeek, YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth )
	{
		if ( calendar == null )
		{
			throw new ArgumentNullException( nameof(calendar) );
		}
		this.calendar = calendar;
		this.yearBaseMonth = yearBaseMonth;
		this.firstDayOfWeek = firstDayOfWeek;
		this.date1 = date1;
		this.date2 = date2;
		difference = date2.Subtract( date1 );
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with a time difference.
	/// The start date is set to the current time and the end date is calculated by adding the difference.
	/// </summary>
	/// <param name="difference">The time difference to use.</param>
	public DateDiff( TimeSpan difference ) :
		this( ClockProxy.Clock.Now, difference, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek )
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with a time difference and calendar settings.
	/// The start date is set to the current time and the end date is calculated by adding the difference.
	/// </summary>
	/// <param name="difference">The time difference to use.</param>
	/// <param name="calendar">The calendar to use for calculations.</param>
	/// <param name="firstDayOfWeek">The first day of the week for week calculations.</param>
	/// <param name="yearBaseMonth">The base month for year calculations.</param>
	public DateDiff( TimeSpan difference, Calendar calendar,
		DayOfWeek firstDayOfWeek, YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth ) :
		this( ClockProxy.Clock.Now, difference, calendar, firstDayOfWeek, yearBaseMonth )
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with a start date and time difference.
	/// The end date is calculated by adding the difference to the start date.
	/// </summary>
	/// <param name="date1">The start date.</param>
	/// <param name="difference">The time difference to add to the start date.</param>
	public DateDiff( DateTime date1, TimeSpan difference ) :
		this( date1, difference, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek )
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DateDiff"/> class with a start date, time difference, and calendar settings.
	/// The end date is calculated by adding the difference to the start date.
	/// </summary>
	/// <param name="date1">The start date.</param>
	/// <param name="difference">The time difference to add to the start date.</param>
	/// <param name="calendar">The calendar to use for calculations.</param>
	/// <param name="firstDayOfWeek">The first day of the week for week calculations.</param>
	/// <param name="yearBaseMonth">The base month for year calculations.</param>
	/// <exception cref="ArgumentNullException">Thrown when calendar is null.</exception>
	public DateDiff( DateTime date1, TimeSpan difference, Calendar calendar,
		DayOfWeek firstDayOfWeek, YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth )
	{
		if ( calendar == null )
		{
			throw new ArgumentNullException( nameof(calendar) );
		}
		this.calendar = calendar;
		this.yearBaseMonth = yearBaseMonth;
		this.firstDayOfWeek = firstDayOfWeek;
		this.date1 = date1;
		date2 = date1.Add( difference );
		this.difference = difference;
	}

	/// <summary>
	/// Gets the safe current date time format info, falling back to invariant info if current info is null.
	/// </summary>
	public static DateTimeFormatInfo SafeCurrentInfo => ( DateTimeFormatInfo.CurrentInfo ?? DateTimeFormatInfo.InvariantInfo );

	/// <summary>
	/// Gets the calendar used for date calculations.
	/// </summary>
	public Calendar Calendar => calendar;
		
	/// <summary>
	/// Gets the base month used for year calculations.
	/// </summary>
	public YearMonth YearBaseMonth => yearBaseMonth;
		
	/// <summary>
	/// Gets the first day of the week used for week calculations.
	/// </summary>
	public DayOfWeek FirstDayOfWeek => firstDayOfWeek;
		
	/// <summary>
	/// Gets the first date (start date) of the difference calculation.
	/// </summary>
	public DateTime Date1 => date1;

	/// <summary>
	/// Gets the second date (end date) of the difference calculation.
	/// </summary>
	public DateTime Date2 => date2;

	/// <summary>
	/// Gets the time span difference between the two dates.
	/// </summary>
	public TimeSpan Difference => difference;

	/// <summary>
	/// Gets a value indicating whether the difference is empty (zero time span).
	/// </summary>
	public bool IsEmpty => difference == TimeSpan.Zero;

	private int Year1 => calendar.GetYear( Date1 );

	private int Year2 => calendar.GetYear( Date2 );

	/// <summary>
	/// Gets the total number of years in the date difference.
	/// </summary>
	public int Years
	{
		get
		{
			if ( !years.HasValue )
			{
				years = CalcYears();
			}
			return years.Value;
		}
	}

	/// <summary>
	/// Gets the number of elapsed years in the date difference.
	/// This is the same as Years for this implementation.
	/// </summary>
	public int ElapsedYears
	{
		get
		{
			if ( !elapsedYears.HasValue )
			{
				elapsedYears = Years;
			}
			return elapsedYears.Value;
		}
	}

	/// <summary>
	/// Gets the total number of quarters in the date difference.
	/// </summary>
	public int Quarters
	{
		get
		{
			if ( !quarters.HasValue )
			{
				quarters = CalcQuarters();
			}
			return quarters.Value;
		}
	}

	private int Month1 => calendar.GetMonth( Date1 );

	private int Month2 => calendar.GetMonth( Date2 );

	/// <summary>
	/// Gets the total number of months in the date difference.
	/// </summary>
	public int Months
	{
		get
		{
			if ( !months.HasValue )
			{
				months = CalcMonths();
			}
			return months.Value;
		}
	}

	/// <summary>
	/// Gets the number of elapsed months in the date difference, excluding full years.
	/// This represents the remaining months after subtracting complete years.
	/// </summary>
	public int ElapsedMonths
	{
		get
		{
			if ( !elapsedMonths.HasValue )
			{
				elapsedMonths = Months - ( ElapsedYears * TimeSpec.MonthsPerYear );
			}
			return elapsedMonths.Value;
		}
	}

	/// <summary>
	/// Gets the total number of weeks in the date difference.
	/// </summary>
	public int Weeks
	{
		get
		{
			if ( !weeks.HasValue )
			{
				weeks = CalcWeeks();
			}
			return weeks.Value;
		}
	}

	/// <summary>
	/// Gets the total number of days in the date difference.
	/// </summary>
	public int Days => (int)Math.Round( Round( difference.TotalDays ) );

	/// <summary>
	/// Gets the number of weekdays (7-day periods) in the date difference.
	/// </summary>
	public int Weekdays => ( (int)Math.Round( Round( difference.TotalDays ) ) ) / TimeSpec.DaysPerWeek;

	/// <summary>
	/// Gets the number of elapsed days in the date difference, excluding full years and months.
	/// This represents the remaining days after subtracting complete years and months.
	/// </summary>
	public int ElapsedDays
	{
		get
		{
			if (elapsedDays.HasValue) return elapsedDays.Value;
			DateTime compareDate = date1.AddYears( ElapsedYears ).AddMonths( ElapsedMonths );
			elapsedDays = (int)date2.Subtract( compareDate ).TotalDays;
			return elapsedDays.Value;
		}
	}

	/// <summary>
	/// Gets the total number of hours in the date difference.
	/// </summary>
	public int Hours => (int)Math.Round( Round( difference.TotalHours ) );

	/// <summary>
	/// Gets the number of elapsed hours in the date difference, excluding full years, months, and days.
	/// This represents the remaining hours after subtracting complete years, months, and days.
	/// </summary>
	public int ElapsedHours
	{
		get
		{
			if (elapsedHours.HasValue) return elapsedHours.Value;
			DateTime compareDate = date1.AddYears( ElapsedYears ).AddMonths( ElapsedMonths ).AddDays( ElapsedDays );
			elapsedHours = (int)date2.Subtract( compareDate ).TotalHours;
			return elapsedHours.Value;
		}
	} // ElapsedHours

	// ----------------------------------------------------------------------
	/// <summary>
	/// Gets the total number of minutes in the date difference.
	/// </summary>
	public int Minutes
	{
		get { return (int)Math.Round( Round( difference.TotalMinutes ) ); }
	} // Minutes

	// ----------------------------------------------------------------------
	/// <summary>
	/// Gets the number of elapsed minutes in the date difference, excluding full years, months, days, and hours.
	/// This represents the remaining minutes after subtracting complete years, months, days, and hours.
	/// </summary>
	public int ElapsedMinutes
	{
		get
		{
			if ( !elapsedMinutes.HasValue )
			{
				DateTime compareDate = date1.AddYears(
					ElapsedYears ).AddMonths( ElapsedMonths ).AddDays( ElapsedDays ).AddHours( ElapsedHours );
				elapsedMinutes = (int)date2.Subtract( compareDate ).TotalMinutes;
			}
			return elapsedMinutes.Value;
		}
	} // ElapsedMinutes

	// ----------------------------------------------------------------------
	/// <summary>
	/// Gets the total number of seconds in the date difference.
	/// </summary>
	public int Seconds
	{
		get { return (int)Math.Round( Round( difference.TotalSeconds ) ); }
	} // Seconds

	// ----------------------------------------------------------------------
	/// <summary>
	/// Gets the number of elapsed seconds in the date difference, excluding all larger time units.
	/// This represents the remaining seconds after subtracting complete years, months, days, hours, and minutes.
	/// </summary>
	public int ElapsedSeconds
	{
		get
		{
			if ( !elapsedSeconds.HasValue )
			{
				DateTime compareDate = date1.AddYears(
					ElapsedYears ).AddMonths(
					ElapsedMonths ).AddDays(
					ElapsedDays ).AddHours(
					ElapsedHours ).AddMinutes(
					ElapsedMinutes );
				elapsedSeconds = (int)date2.Subtract( compareDate ).TotalSeconds;
			}
			return elapsedSeconds.Value;
		}
	} // ElapsedSeconds

	// ----------------------------------------------------------------------
	/// <summary>
	/// Gets a formatted description of the date difference using the specified precision and formatter.
	/// </summary>
	/// <param name="precision">The maximum number of time units to include in the description. Must be at least 1.</param>
	/// <param name="formatter">The time formatter to use. If null, uses the default TimeFormatter.Instance.</param>
	/// <returns>A formatted string describing the date difference.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when precision is less than 1.</exception>
	public string GetDescription( int precision = int.MaxValue, ITimeFormatter formatter = null )
	{
		if ( precision < 1 )
		{
			throw new ArgumentOutOfRangeException( "precision" );
		}

		formatter = formatter ?? TimeFormatter.Instance;

		int[] elapsedItems = new int[ 6 ];
		elapsedItems[ 0 ] = ElapsedYears;
		elapsedItems[ 1 ] = ElapsedMonths;
		elapsedItems[ 2 ] = ElapsedDays;
		elapsedItems[ 3 ] = ElapsedHours;
		elapsedItems[ 4 ] = ElapsedMinutes;
		elapsedItems[ 5 ] = ElapsedSeconds;

		if ( precision <= elapsedItems.Length - 1 )
		{
			for ( int i = precision; i < elapsedItems.Length; i++ )
			{
				elapsedItems[ i ] = 0;
			}
		}

		return formatter.GetDuration(
			elapsedItems[ 0 ],
			elapsedItems[ 1 ],
			elapsedItems[ 2 ],
			elapsedItems[ 3 ],
			elapsedItems[ 4 ],
			elapsedItems[ 5 ] );
	} // GetDescription

	// ----------------------------------------------------------------------
	/// <summary>
	/// Returns a string representation of the date difference using the default formatter.
	/// </summary>
	/// <returns>A formatted string describing the date difference.</returns>
	public override string ToString()
	{
		return GetDescription();
	} // ToString

	// ----------------------------------------------------------------------
	/// <summary>
	/// Determines whether the specified object is equal to the current DateDiff instance.
	/// </summary>
	/// <param name="obj">The object to compare with the current instance.</param>
	/// <returns>true if the specified object is equal to the current instance; otherwise, false.</returns>
	public override bool Equals( object obj )
	{
		if ( obj == this )
		{
			return true;
		}
		if ( obj == null || GetType() != obj.GetType() )
		{
			return false;
		}

		DateDiff comp = (DateDiff)obj;
		return calendar == comp.calendar &&
		       yearBaseMonth == comp.yearBaseMonth &&
		       firstDayOfWeek == comp.firstDayOfWeek &&
		       date1 == comp.date1 &&
		       date2 == comp.date2 &&
		       difference == comp.difference;
	} // Equals

	// ----------------------------------------------------------------------
	/// <summary>
	/// Returns the hash code for this DateDiff instance.
	/// </summary>
	/// <returns>A 32-bit signed integer hash code.</returns>
	public override int GetHashCode()
	{
		return HashTool.ComputeHashCode( GetType().GetHashCode(),
			calendar,
			yearBaseMonth,
			firstDayOfWeek,
			date1,
			date2,
			difference );
	} // GetHashCode

	// ----------------------------------------------------------------------
	private static double Round( double number )
	{
		if ( number >= 0.0 )
		{
			return Math.Floor( number );
		}
		return -Math.Floor( -number );
	} // Round

	// ----------------------------------------------------------------------
	private int CalcYears()
	{
		if ( TimeCompare.IsSameMonth( date1, date2 ) )
		{
			return 0;
		}

		int compareDay = date2.Day;
		int compareDaysPerMonth = calendar.GetDaysInMonth( Year1, Month2 );
		if ( compareDay > compareDaysPerMonth )
		{
			compareDay = compareDaysPerMonth;
		}
		DateTime compareDate = new DateTime( Year1, Month2, compareDay,
			date2.Hour, date2.Minute, date2.Second, date2.Millisecond );
		if ( date2 > date1 )
		{
			if ( compareDate < date1 )
			{
				compareDate = compareDate.AddYears( 1 );
			}
		}
		else
		{
			if ( compareDate > date1 )
			{
				compareDate = compareDate.AddYears( -1 );
			}
		}
		return Year2 - calendar.GetYear( compareDate );
	} // CalcYears

	private int CalcQuarters()
	{
		if ( TimeCompare.IsSameMonth( date1, date2 ) )
		{
			return 0;
		}

		int year1 = TimeTool.GetYearOf( yearBaseMonth, Year1, Month1 );
		YearQuarter quarter1 = TimeTool.GetQuarterOfMonth( yearBaseMonth, (YearMonth)Month1 );

		int year2 = TimeTool.GetYearOf( yearBaseMonth, Year2, Month2 );
		YearQuarter quarter2 = TimeTool.GetQuarterOfMonth( yearBaseMonth, (YearMonth)Month2 );

		return
			( ( year2 * TimeSpec.QuartersPerYear ) + quarter2 ) -
			( ( year1 * TimeSpec.QuartersPerYear ) + quarter1 );
	} // CalcQuarters

	private int CalcMonths()
	{
		if ( TimeCompare.IsSameDay( date1, date2 ) )
		{
			return 0;
		}

		int compareDay = date2.Day;
		int compareDaysPerMonth = calendar.GetDaysInMonth( Year1, Month1 );
		if ( compareDay > compareDaysPerMonth )
		{
			compareDay = compareDaysPerMonth;
		}

		DateTime compareDate = new DateTime( Year1, Month1, compareDay,
			date2.Hour, date2.Minute, date2.Second, date2.Millisecond );
		if ( date2 > date1 )
		{
			if ( compareDate < date1 )
			{
				compareDate = compareDate.AddMonths( 1 );
			}
		}
		else
		{
			if ( compareDate > date1 )
			{
				compareDate = compareDate.AddMonths( -1 );
			}
		}
		return
			( ( Year2 * TimeSpec.MonthsPerYear ) + Month2 ) -
			( ( calendar.GetYear( compareDate ) * TimeSpec.MonthsPerYear ) + calendar.GetMonth( compareDate ) );
	} // CalcMonths

	private int CalcWeeks()
	{
		if ( TimeCompare.IsSameDay( date1, date2 ) )
		{
			return 0;
		}

		DateTime week1 = TimeTool.GetStartOfWeek( date1, firstDayOfWeek );
		DateTime week2 = TimeTool.GetStartOfWeek( date2, firstDayOfWeek );
		if ( week1.Equals( week2 ) )
		{
			return 0;
		}

		return (int)( week2.Subtract( week1 ).TotalDays / TimeSpec.DaysPerWeek );
	} // CalcWeeks

	private readonly Calendar calendar;
	private readonly YearMonth yearBaseMonth;
	private readonly DayOfWeek firstDayOfWeek;
	private readonly DateTime date1;
	private readonly DateTime date2;
	private readonly TimeSpan difference;
	// cached values
	private int? years;
	private int? quarters;
	private int? months;
	private int? weeks;
	private int? elapsedYears;
	private int? elapsedMonths;
	private int? elapsedDays;
	private int? elapsedHours;
	private int? elapsedMinutes;
	private int? elapsedSeconds;

}