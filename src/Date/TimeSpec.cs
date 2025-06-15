using System;

namespace eQuantic.Core.Date;

/// <summary>
/// Provides time-related constants and specifications for calendar and fiscal calculations.
/// </summary>
public static class TimeSpec
{
	/// <summary>
	/// The number of months in a year.
	/// </summary>
	public const int MonthsPerYear = 12;
	
	/// <summary>
	/// The number of half-years in a year.
	/// </summary>
	public const int HalfyearsPerYear = 2;
	
	/// <summary>
	/// The number of quarters in a year.
	/// </summary>
	public const int QuartersPerYear = 4;
	
	/// <summary>
	/// The number of quarters in a half-year.
	/// </summary>
	public const int QuartersPerHalfyear = QuartersPerYear / HalfyearsPerYear;
	
	/// <summary>
	/// The maximum number of weeks in a year.
	/// </summary>
	public const int MaxWeeksPerYear = 53;
	
	/// <summary>
	/// The number of months in a half-year.
	/// </summary>
	public const int MonthsPerHalfyear = MonthsPerYear / HalfyearsPerYear;
	
	/// <summary>
	/// The number of months in a quarter.
	/// </summary>
	public const int MonthsPerQuarter = MonthsPerYear / QuartersPerYear;
	
	/// <summary>
	/// The maximum number of days in a month.
	/// </summary>
	public const int MaxDaysPerMonth = 31;
	
	/// <summary>
	/// The number of days in a week.
	/// </summary>
	public const int DaysPerWeek = 7;
	
	/// <summary>
	/// The number of hours in a day.
	/// </summary>
	public const int HoursPerDay = 24;
	
	/// <summary>
	/// The number of minutes in an hour.
	/// </summary>
	public const int MinutesPerHour = 60;
	
	/// <summary>
	/// The number of seconds in a minute.
	/// </summary>
	public const int SecondsPerMinute = 60;
	
	/// <summary>
	/// The number of milliseconds in a second.
	/// </summary>
	public const int MillisecondsPerSecond = 1000;

	/// <summary>
	/// The starting month for calendar year calculations.
	/// </summary>
	public const YearMonth CalendarYearStartMonth = YearMonth.January;
	
	/// <summary>
	/// The first working day of the week.
	/// </summary>
	public const DayOfWeek FirstWorkingDayOfWeek = DayOfWeek.Monday;

	/// <summary>
	/// The base month for fiscal year calculations.
	/// </summary>
	public const YearMonth FiscalYearBaseMonth = YearMonth.July;
	
	/// <summary>
	/// The number of weeks in a short fiscal month.
	/// </summary>
	public const int FiscalWeeksPerShortMonth = 4;
	
	/// <summary>
	/// The number of weeks in a long fiscal month.
	/// </summary>
	public const int FiscalWeeksPerLongMonth = 5;
	
	/// <summary>
	/// The number of weeks in a leap fiscal month.
	/// </summary>
	public const int FiscalWeeksPerLeapMonth = 6;
	
	/// <summary>
	/// The number of weeks in a fiscal quarter.
	/// </summary>
	public const int FiscalWeeksPerQuarter = ( 2 * FiscalWeeksPerShortMonth ) + FiscalWeeksPerLongMonth;
	
	/// <summary>
	/// The number of weeks in a leap fiscal quarter.
	/// </summary>
	public const int FiscalWeeksPerLeapQuarter = FiscalWeeksPerQuarter + 1;
	
	/// <summary>
	/// The number of weeks in a fiscal half-year.
	/// </summary>
	public const int FiscalWeeksPerHalfyear = FiscalWeeksPerQuarter * QuartersPerHalfyear;
	
	/// <summary>
	/// The number of weeks in a leap fiscal half-year.
	/// </summary>
	public const int FiscalWeeksPerLeapHalfyear = FiscalWeeksPerHalfyear + 1;
	
	/// <summary>
	/// The number of weeks in a fiscal year.
	/// </summary>
	public const int FiscalWeeksPerYear = FiscalWeeksPerQuarter * QuartersPerYear;
	
	/// <summary>
	/// The number of weeks in a leap fiscal year.
	/// </summary>
	public const int FiscalWeeksPerLeapYear = FiscalWeeksPerYear + 1;

	/// <summary>
	/// The number of days in a short fiscal month.
	/// </summary>
	public const int FiscalDaysPerShortMonth = FiscalWeeksPerShortMonth * DaysPerWeek;
	
	/// <summary>
	/// The number of days in a long fiscal month.
	/// </summary>
	public const int FiscalDaysPerLongMonth = FiscalWeeksPerLongMonth * DaysPerWeek;
	
	/// <summary>
	/// The number of days in a leap fiscal month.
	/// </summary>
	public const int FiscalDaysPerLeapMonth = FiscalWeeksPerLeapMonth * DaysPerWeek;
	
	/// <summary>
	/// The number of days in a fiscal quarter.
	/// </summary>
	public const int FiscalDaysPerQuarter = ( 2 * FiscalDaysPerShortMonth ) + FiscalDaysPerLongMonth;
	
	/// <summary>
	/// The number of days in a leap fiscal quarter.
	/// </summary>
	public const int FiscalDaysPerLeapQuarter = FiscalDaysPerQuarter + DaysPerWeek;
	
	/// <summary>
	/// The number of days in a fiscal half-year.
	/// </summary>
	public const int FiscalDaysPerHalfyear = FiscalDaysPerQuarter * QuartersPerHalfyear;
	
	/// <summary>
	/// The number of days in a leap fiscal half-year.
	/// </summary>
	public const int FiscalDaysPerLeapHalfyear = FiscalDaysPerHalfyear + DaysPerWeek;
	
	/// <summary>
	/// The number of days in a fiscal year.
	/// </summary>
	public const int FiscalDaysPerYear = FiscalDaysPerQuarter * QuartersPerYear;
	
	/// <summary>
	/// The number of days in a leap fiscal year.
	/// </summary>
	public const int FiscalDaysPerLeapYear = FiscalDaysPerYear + DaysPerWeek;

	/// <summary>
	/// Array containing the months of the first half-year.
	/// </summary>
	public static YearMonth[] FirstHalfyearMonths = new[] { YearMonth.January, YearMonth.February, YearMonth.March, YearMonth.April, YearMonth.May, YearMonth.June };
	
	/// <summary>
	/// Array containing the months of the second half-year.
	/// </summary>
	public static YearMonth[] SecondHalfyearMonths = new[] { YearMonth.July, YearMonth.August, YearMonth.September, YearMonth.October, YearMonth.November, YearMonth.December };

	/// <summary>
	/// The starting month index for the first quarter.
	/// </summary>
	public const int FirstQuarterMonthIndex = 1;
	
	/// <summary>
	/// The starting month index for the second quarter.
	/// </summary>
	public const int SecondQuarterMonthIndex = FirstQuarterMonthIndex + MonthsPerQuarter;
	
	/// <summary>
	/// The starting month index for the third quarter.
	/// </summary>
	public const int ThirdQuarterMonthIndex = SecondQuarterMonthIndex + MonthsPerQuarter;
	
	/// <summary>
	/// The starting month index for the fourth quarter.
	/// </summary>
	public const int FourthQuarterMonthIndex = ThirdQuarterMonthIndex + MonthsPerQuarter;

	/// <summary>
	/// Array containing the months of the first quarter.
	/// </summary>
	public static YearMonth[] FirstQuarterMonths = new[] { YearMonth.January, YearMonth.February, YearMonth.March };
	
	/// <summary>
	/// Array containing the months of the second quarter.
	/// </summary>
	public static YearMonth[] SecondQuarterMonths = new[] { YearMonth.April, YearMonth.May, YearMonth.June };
	
	/// <summary>
	/// Array containing the months of the third quarter.
	/// </summary>
	public static YearMonth[] ThirdQuarterMonths = new[] { YearMonth.July, YearMonth.August, YearMonth.September };
	
	/// <summary>
	/// Array containing the months of the fourth quarter.
	/// </summary>
	public static YearMonth[] FourthQuarterMonths = new[] { YearMonth.October, YearMonth.November, YearMonth.December };

	/// <summary>
	/// Represents a duration of zero time.
	/// </summary>
	public static readonly TimeSpan NoDuration = TimeSpan.Zero;
	
	/// <summary>
	/// The minimum positive duration (one tick).
	/// </summary>
	public static readonly TimeSpan MinPositiveDuration = new TimeSpan( 1 ); // positive tick;
	
	/// <summary>
	/// The minimum negative duration (negative one tick).
	/// </summary>
	public static readonly TimeSpan MinNegativeDuration = new TimeSpan( -1 ); // negative tick;

	/// <summary>
	/// The minimum date value for period calculations.
	/// </summary>
	public static readonly DateTime MinPeriodDate = DateTime.MinValue;
	
	/// <summary>
	/// The maximum date value for period calculations.
	/// </summary>
	public static readonly DateTime MaxPeriodDate = DateTime.MaxValue;
	
	/// <summary>
	/// The minimum duration for period calculations.
	/// </summary>
	public static readonly TimeSpan MinPeriodDuration = TimeSpan.Zero;
	
	/// <summary>
	/// The maximum duration for period calculations.
	/// </summary>
	public static readonly TimeSpan MaxPeriodDuration = MaxPeriodDate - MinPeriodDate;

}