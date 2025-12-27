using System;
using System.Globalization;

namespace eQuantic.Core.Date;

/// <summary>
/// Provides utility methods for date and time manipulation, including fiscal year calculations, 
/// week operations, and various time period conversions.
/// </summary>
public static class TimeTool
{

    #region Date and Time

    /// <summary>
    /// Gets the date component of a DateTime, removing the time portion.
    /// </summary>
    /// <param name="dateTime">The DateTime to extract the date from.</param>
    /// <returns>A DateTime with the same date but with time set to midnight.</returns>
    public static DateTime GetDate(DateTime dateTime)
    {
        return dateTime.Date;
    } // GetDate

    /// <summary>
    /// Sets the date component of a DateTime while preserving the time component.
    /// </summary>
    /// <param name="from">The DateTime whose time component to preserve.</param>
    /// <param name="to">The DateTime whose date component to use.</param>
    /// <returns>A new DateTime with the date from 'to' and time from 'from'.</returns>
    public static DateTime SetDate(DateTime from, DateTime to)
    {
        return SetDate(from, to.Year, to.Month, to.Day);
    } // SetDate

    /// <summary>
    /// Sets the date component of a DateTime while preserving the time component.
    /// </summary>
    /// <param name="from">The DateTime whose time component to preserve.</param>
    /// <param name="year">The year to set.</param>
    /// <param name="month">The month to set. Defaults to 1.</param>
    /// <param name="day">The day to set. Defaults to 1.</param>
    /// <returns>A new DateTime with the specified date and time from 'from'.</returns>
    public static DateTime SetDate(DateTime from, int year, int month = 1, int day = 1)
    {
        return new DateTime(year, month, day, from.Hour, from.Minute, from.Second, from.Millisecond);
    } // SetDate

    /// <summary>
    /// Determines whether a DateTime has a time component other than midnight.
    /// </summary>
    /// <param name="dateTime">The DateTime to check.</param>
    /// <returns>True if the DateTime has a time component greater than zero; otherwise, false.</returns>
    public static bool HasTimeOfDay(DateTime dateTime)
    {
        return dateTime.TimeOfDay > TimeSpan.Zero;
    } // HasTimeOfDay

    /// <summary>
    /// Sets the time component of a DateTime while preserving the date component.
    /// </summary>
    /// <param name="from">The DateTime whose date component to preserve.</param>
    /// <param name="to">The DateTime whose time component to use.</param>
    /// <returns>A new DateTime with the date from 'from' and time from 'to'.</returns>
    public static DateTime SetTimeOfDay(DateTime from, DateTime to)
    {
        return SetTimeOfDay(from, to.Hour, to.Minute, to.Second, to.Millisecond);
    } // SetTimeOfDay

    /// <summary>
    /// Sets the time component of a DateTime while preserving the date component.
    /// </summary>
    /// <param name="from">The DateTime whose date component to preserve.</param>
    /// <param name="hour">The hour to set. Defaults to 0.</param>
    /// <param name="minute">The minute to set. Defaults to 0.</param>
    /// <param name="second">The second to set. Defaults to 0.</param>
    /// <param name="millisecond">The millisecond to set. Defaults to 0.</param>
    /// <returns>A new DateTime with the date from 'from' and the specified time.</returns>
    public static DateTime SetTimeOfDay(DateTime from, int hour = 0, int minute = 0, int second = 0,
        int millisecond = 0)
    {
        return new DateTime(from.Year, from.Month, from.Day, hour, minute, second, millisecond);
    } // SetTimeOfDay

    #endregion

    #region Year

    /// <summary>
    /// Gets the fiscal year for a given DateTime based on the specified year base month.
    /// </summary>
    /// <param name="yearBaseMonth">The month that starts the fiscal year.</param>
    /// <param name="moment">The DateTime to get the fiscal year for.</param>
    /// <returns>The fiscal year.</returns>
    public static int GetYearOf(YearMonth yearBaseMonth, DateTime moment)
    {
        return GetYearOf(yearBaseMonth, moment.Year, moment.Month);
    } // GetYearOf

    /// <summary>
    /// Gets the fiscal year for a given year and month based on the specified year base month.
    /// </summary>
    /// <param name="yearBaseMonth">The month that starts the fiscal year.</param>
    /// <param name="year">The calendar year.</param>
    /// <param name="month">The month.</param>
    /// <returns>The fiscal year.</returns>
    public static int GetYearOf(YearMonth yearBaseMonth, int year, int month)
    {
        return month >= (int) yearBaseMonth ? year : year - 1;
    } // GetYearOf

    #endregion

    #region Halfyear

    /// <summary>
    /// Gets the next half-year from the specified starting half-year.
    /// </summary>
    /// <param name="startHalfyear">The starting half-year.</param>
    /// <param name="year">When this method returns, contains the year of the next half-year.</param>
    /// <param name="halfyear">When this method returns, contains the next half-year.</param>
    public static void NextHalfyear(YearHalfyear startHalfyear, out int year, out YearHalfyear halfyear)
    {
        AddHalfyear(startHalfyear, 1, out year, out halfyear);
    } // NextHalfyear

    /// <summary>
    /// Gets the previous half-year from the specified starting half-year.
    /// </summary>
    /// <param name="startHalfyear">The starting half-year.</param>
    /// <param name="year">When this method returns, contains the year of the previous half-year.</param>
    /// <param name="halfyear">When this method returns, contains the previous half-year.</param>
    public static void PreviousHalfyear(YearHalfyear startHalfyear, out int year, out YearHalfyear halfyear)
    {
        AddHalfyear(startHalfyear, -1, out year, out halfyear);
    } // PreviousHalfyear

    /// <summary>
    /// Adds the specified number of half-years to the starting half-year.
    /// </summary>
    /// <param name="startHalfyear">The starting half-year.</param>
    /// <param name="count">The number of half-years to add (can be negative).</param>
    /// <param name="year">When this method returns, contains the resulting year.</param>
    /// <param name="halfyear">When this method returns, contains the resulting half-year.</param>
    public static void AddHalfyear(YearHalfyear startHalfyear, int count, out int year, out YearHalfyear halfyear)
    {
        AddHalfyear(0, startHalfyear, count, out year, out halfyear);
    } // AddHalfyear

    /// <summary>
    /// Adds the specified number of half-years to the starting year and half-year.
    /// </summary>
    /// <param name="startYear">The starting year.</param>
    /// <param name="startHalfyear">The starting half-year.</param>
    /// <param name="count">The number of half-years to add (can be negative).</param>
    /// <param name="year">When this method returns, contains the resulting year.</param>
    /// <param name="halfyear">When this method returns, contains the resulting half-year.</param>
    public static void AddHalfyear(int startYear, YearHalfyear startHalfyear, int count, out int year,
        out YearHalfyear halfyear)
    {
        int offsetYear = (Math.Abs(count)/TimeSpec.HalfyearsPerYear) + 1;
        int startHalfyearCount = ((startYear + offsetYear)*TimeSpec.HalfyearsPerYear) + ((int) startHalfyear - 1);
        int targetHalfyearCount = startHalfyearCount + count;

        year = (targetHalfyearCount/TimeSpec.HalfyearsPerYear) - offsetYear;
        halfyear = (YearHalfyear) ((targetHalfyearCount%TimeSpec.HalfyearsPerYear) + 1);
    } // AddHalfyear

    /// <summary>
    /// Gets the half-year that contains the specified month using calendar year.
    /// </summary>
    /// <param name="yearMonth">The month to get the half-year for.</param>
    /// <returns>The half-year that contains the specified month.</returns>
    public static YearHalfyear GetHalfyearOfMonth(YearMonth yearMonth)
    {
        return GetHalfyearOfMonth(TimeSpec.CalendarYearStartMonth, yearMonth);
    } // GetHalfyearOfMonth

    /// <summary>
    /// Gets the half-year that contains the specified month based on the fiscal year start month.
    /// </summary>
    /// <param name="yearBaseMonth">The month that starts the fiscal year.</param>
    /// <param name="yearMonth">The month to get the half-year for.</param>
    /// <returns>The half-year that contains the specified month.</returns>
    public static YearHalfyear GetHalfyearOfMonth(YearMonth yearBaseMonth, YearMonth yearMonth)
    {
        int yearMonthIndex = (int) yearMonth - 1;
        int yearStartMonthIndex = (int) yearBaseMonth - 1;
        if (yearMonthIndex < yearStartMonthIndex)
        {
            yearMonthIndex += TimeSpec.MonthsPerYear;
        }
        int deltaMonths = yearMonthIndex - yearStartMonthIndex;
        return (YearHalfyear) ((deltaMonths/TimeSpec.MonthsPerHalfyear) + 1);
    } // GetHalfyearOfMonth

    /// <summary>
    /// Gets the months that belong to the specified half-year.
    /// </summary>
    /// <param name="yearHalfyear">The half-year to get the months for.</param>
    /// <returns>An array of months that belong to the specified half-year.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid half-year is specified.</exception>
    public static YearMonth[] GetMonthsOfHalfyear(YearHalfyear yearHalfyear)
    {
        switch (yearHalfyear)
        {
            case YearHalfyear.First:
                return TimeSpec.FirstHalfyearMonths;
            case YearHalfyear.Second:
                return TimeSpec.SecondHalfyearMonths;
        }
        throw new InvalidOperationException("invalid year halfyear " + yearHalfyear);
    } // GetMonthsOfHalfyear

    #endregion

    #region Quarter

    /// <summary>
    /// Gets the next quarter from the specified starting quarter.
    /// </summary>
    /// <param name="startQuarter">The starting quarter.</param>
    /// <param name="year">When this method returns, contains the year of the next quarter.</param>
    /// <param name="quarter">When this method returns, contains the next quarter.</param>
    public static void NextQuarter(YearQuarter startQuarter, out int year, out YearQuarter quarter)
    {
        AddQuarter(startQuarter, 1, out year, out quarter);
    } // NextQuarter

    /// <summary>
    /// Gets the previous quarter from the specified starting quarter.
    /// </summary>
    /// <param name="startQuarter">The starting quarter.</param>
    /// <param name="year">When this method returns, contains the year of the previous quarter.</param>
    /// <param name="quarter">When this method returns, contains the previous quarter.</param>
    public static void PreviousQuarter(YearQuarter startQuarter, out int year, out YearQuarter quarter)
    {
        AddQuarter(startQuarter, -1, out year, out quarter);
    } // PreviousQuarter

    /// <summary>
    /// Adds the specified number of quarters to the starting quarter.
    /// </summary>
    /// <param name="startQuarter">The starting quarter.</param>
    /// <param name="count">The number of quarters to add (can be negative).</param>
    /// <param name="year">When this method returns, contains the resulting year.</param>
    /// <param name="quarter">When this method returns, contains the resulting quarter.</param>
    public static void AddQuarter(YearQuarter startQuarter, int count, out int year, out YearQuarter quarter)
    {
        AddQuarter(0, startQuarter, count, out year, out quarter);
    } // AddQuarter

    /// <summary>
    /// Adds the specified number of quarters to the starting year and quarter.
    /// </summary>
    /// <param name="startYear">The starting year.</param>
    /// <param name="startQuarter">The starting quarter.</param>
    /// <param name="count">The number of quarters to add (can be negative).</param>
    /// <param name="year">When this method returns, contains the resulting year.</param>
    /// <param name="quarter">When this method returns, contains the resulting quarter.</param>
    public static void AddQuarter(int startYear, YearQuarter startQuarter, int count, out int year,
        out YearQuarter quarter)
    {
        int offsetYear = (Math.Abs(count)/TimeSpec.QuartersPerYear) + 1;
        int startQuarterCount = ((startYear + offsetYear)*TimeSpec.QuartersPerYear) + ((int) startQuarter - 1);
        int targetQuarterCount = startQuarterCount + count;

        year = (targetQuarterCount/TimeSpec.QuartersPerYear) - offsetYear;
        quarter = (YearQuarter) ((targetQuarterCount%TimeSpec.QuartersPerYear) + 1);
    } // AddQuarter

    /// <summary>
    /// Gets the quarter that contains the specified month using calendar year.
    /// </summary>
    /// <param name="yearMonth">The month to get the quarter for.</param>
    /// <returns>The quarter that contains the specified month.</returns>
    public static YearQuarter GetQuarterOfMonth(YearMonth yearMonth)
    {
        return GetQuarterOfMonth(TimeSpec.CalendarYearStartMonth, yearMonth);
    } // GetQuarterOfMonth

    /// <summary>
    /// Gets the quarter that contains the specified month based on the fiscal year start month.
    /// </summary>
    /// <param name="yearBaseMonth">The month that starts the fiscal year.</param>
    /// <param name="yearMonth">The month to get the quarter for.</param>
    /// <returns>The quarter that contains the specified month.</returns>
    public static YearQuarter GetQuarterOfMonth(YearMonth yearBaseMonth, YearMonth yearMonth)
    {
        int yearMonthIndex = (int) yearMonth - 1;
        int yearStartMonthIndex = (int) yearBaseMonth - 1;
        if (yearMonthIndex < yearStartMonthIndex)
        {
            yearMonthIndex += TimeSpec.MonthsPerYear;
        }
        int deltaMonths = yearMonthIndex - yearStartMonthIndex;
        return (YearQuarter) ((deltaMonths/TimeSpec.MonthsPerQuarter) + 1);
    } // GetQuarterOfMonth

    /// <summary>
    /// Gets the months that belong to the specified quarter.
    /// </summary>
    /// <param name="yearQuarter">The quarter to get the months for.</param>
    /// <returns>An array of months that belong to the specified quarter.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid quarter is specified.</exception>
    public static YearMonth[] GetMonthsOfQuarter(YearQuarter yearQuarter)
    {
        switch (yearQuarter)
        {
            case YearQuarter.First:
                return TimeSpec.FirstQuarterMonths;
            case YearQuarter.Second:
                return TimeSpec.SecondQuarterMonths;
            case YearQuarter.Third:
                return TimeSpec.ThirdQuarterMonths;
            case YearQuarter.Fourth:
                return TimeSpec.FourthQuarterMonths;
        }
        throw new InvalidOperationException("invalid year quarter " + yearQuarter);
    } // GetMonthsOfQuarter

    #endregion

    #region Month

    /// <summary>
    /// Gets the next month from the specified starting month.
    /// </summary>
    /// <param name="startMonth">The starting month.</param>
    /// <param name="year">When this method returns, contains the year of the next month.</param>
    /// <param name="month">When this method returns, contains the next month.</param>
    public static void NextMonth(YearMonth startMonth, out int year, out YearMonth month)
    {
        AddMonth(startMonth, 1, out year, out month);
    } // NextMonth

    /// <summary>
    /// Gets the previous month from the specified starting month.
    /// </summary>
    /// <param name="startMonth">The starting month.</param>
    /// <param name="year">When this method returns, contains the year of the previous month.</param>
    /// <param name="month">When this method returns, contains the previous month.</param>
    public static void PreviousMonth(YearMonth startMonth, out int year, out YearMonth month)
    {
        AddMonth(startMonth, -1, out year, out month);
    } // PreviousMonth

    /// <summary>
    /// Adds the specified number of months to the starting month.
    /// </summary>
    /// <param name="startMonth">The starting month.</param>
    /// <param name="count">The number of months to add (can be negative).</param>
    /// <param name="year">When this method returns, contains the resulting year.</param>
    /// <param name="month">When this method returns, contains the resulting month.</param>
    public static void AddMonth(YearMonth startMonth, int count, out int year, out YearMonth month)
    {
        AddMonth(0, startMonth, count, out year, out month);
    } // AddMonth

    /// <summary>
    /// Adds the specified number of months to the starting year and month.
    /// </summary>
    /// <param name="startYear">The starting year.</param>
    /// <param name="startMonth">The starting month.</param>
    /// <param name="count">The number of months to add (can be negative).</param>
    /// <param name="year">When this method returns, contains the resulting year.</param>
    /// <param name="month">When this method returns, contains the resulting month.</param>
    public static void AddMonth(int startYear, YearMonth startMonth, int count, out int year, out YearMonth month)
    {
        int offsetYear = (Math.Abs(count)/TimeSpec.MonthsPerYear) + 1;
        int startMonthCount = ((startYear + offsetYear)*TimeSpec.MonthsPerYear) + ((int) startMonth - 1);
        int targetMonthCount = startMonthCount + count;

        year = (targetMonthCount/TimeSpec.MonthsPerYear) - offsetYear;
        month = (YearMonth) ((targetMonthCount%TimeSpec.MonthsPerYear) + 1);
    } // AddMonth

    /// <summary>
    /// Gets the number of days in the specified month and year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month (1-12).</param>
    /// <returns>The number of days in the specified month and year.</returns>
    public static int GetDaysInMonth(int year, int month)
    {
        DateTime firstDay = new DateTime(year, month, 1);
        return firstDay.AddMonths(1).AddDays(-1).Day;
    } // GetDaysInMonth

    #endregion

    #region Week

    /// <summary>
    /// Gets the start date of the week that contains the specified date.
    /// </summary>
    /// <param name="time">The date to get the week start for.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    /// <returns>The DateTime representing the start of the week.</returns>
    public static DateTime GetStartOfWeek(DateTime time, DayOfWeek firstDayOfWeek)
    {
        DateTime currentDay = new DateTime(time.Year, time.Month, time.Day);
        while (currentDay.DayOfWeek != firstDayOfWeek)
        {
            currentDay = currentDay.AddDays(-1);
        }
        return currentDay;
    } // GetStartOfWeek

    /// <summary>
    /// Gets the week number and year for the specified date using culture-specific settings.
    /// </summary>
    /// <param name="moment">The date to get the week information for.</param>
    /// <param name="culture">The culture to use for week calculations.</param>
    /// <param name="yearWeekType">The type of week calculation to use.</param>
    /// <param name="year">When this method returns, contains the year of the week.</param>
    /// <param name="weekOfYear">When this method returns, contains the week number within the year.</param>
    public static void GetWeekOfYear(DateTime moment, CultureInfo culture, YearWeekType yearWeekType,
        out int year, out int weekOfYear)
    {
        GetWeekOfYear(moment, culture, culture.DateTimeFormat.CalendarWeekRule,
            culture.DateTimeFormat.FirstDayOfWeek, yearWeekType,
            out year, out weekOfYear);
    } // GetWeekOfYear

    /// <summary>
    /// Gets the week number and year for the specified date using specific week calculation rules.
    /// </summary>
    /// <param name="moment">The date to get the week information for.</param>
    /// <param name="culture">The culture to use for week calculations.</param>
    /// <param name="weekRule">The calendar week rule to use.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    /// <param name="yearWeekType">The type of week calculation to use.</param>
    /// <param name="year">When this method returns, contains the year of the week.</param>
    /// <param name="weekOfYear">When this method returns, contains the week number within the year.</param>
    /// <exception cref="ArgumentNullException">Thrown when culture is null.</exception>
    public static void GetWeekOfYear(DateTime moment, CultureInfo culture,
        CalendarWeekRule weekRule, DayOfWeek firstDayOfWeek, YearWeekType yearWeekType, out int year,
        out int weekOfYear)
    {
        if (culture == null)
        {
            throw new ArgumentNullException(nameof(culture));
        }

        if (yearWeekType == YearWeekType.Iso8601 && weekRule == CalendarWeekRule.FirstFourDayWeek)
        {
            // see http://blogs.msdn.com/b/shawnste/archive/2006/01/24/517178.aspx
            DayOfWeek day = culture.Calendar.GetDayOfWeek(moment);
            if (day >= firstDayOfWeek && (int) day <= (int) (firstDayOfWeek + 2)%7)
            {
                moment = moment.AddDays(3);
            }
        }

        weekOfYear = culture.Calendar.GetWeekOfYear(moment, weekRule, firstDayOfWeek);
        year = moment.Year;
        if (weekOfYear >= 52 && moment.Month < 12)
        {
            year--;
        }
    } // GetWeekOfYear

    /// <summary>
    /// Gets the total number of weeks in the specified year using culture-specific settings.
    /// </summary>
    /// <param name="year">The year to get the week count for.</param>
    /// <param name="culture">The culture to use for week calculations.</param>
    /// <param name="yearWeekType">The type of week calculation to use.</param>
    /// <returns>The total number of weeks in the specified year.</returns>
    public static int GetWeeksOfYear(int year, CultureInfo culture, YearWeekType yearWeekType)
    {
        return GetWeeksOfYear(year, culture, culture.DateTimeFormat.CalendarWeekRule,
            culture.DateTimeFormat.FirstDayOfWeek, yearWeekType);
    } // GetWeeksOfYear

    /// <summary>
    /// Gets the total number of weeks in the specified year using specific week calculation rules.
    /// </summary>
    /// <param name="year">The year to get the week count for.</param>
    /// <param name="culture">The culture to use for week calculations.</param>
    /// <param name="weekRule">The calendar week rule to use.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    /// <param name="yearWeekType">The type of week calculation to use.</param>
    /// <returns>The total number of weeks in the specified year.</returns>
    /// <exception cref="ArgumentNullException">Thrown when culture is null.</exception>
    public static int GetWeeksOfYear(int year, CultureInfo culture,
        CalendarWeekRule weekRule, DayOfWeek firstDayOfWeek, YearWeekType yearWeekType)
    {
        if (culture == null)
        {
            throw new ArgumentNullException(nameof(culture));
        }

        int currentYear;
        int currentWeek;
        DateTime currentDay = new DateTime(year, 12, 31);
        GetWeekOfYear(currentDay, culture, weekRule, firstDayOfWeek, yearWeekType, out currentYear, out currentWeek);
        while (currentYear != year)
        {
            currentDay = currentDay.AddDays(-1);
            GetWeekOfYear(currentDay, culture, weekRule, firstDayOfWeek, yearWeekType, out currentYear,
                out currentWeek);
        }
        return currentWeek;
    } // GetWeeksOfYear

    /// <summary>
    /// Gets the start date of the specified week in the given year using culture-specific settings.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="weekOfYear">The week number within the year.</param>
    /// <param name="culture">The culture to use for week calculations.</param>
    /// <param name="yearWeekType">The type of week calculation to use.</param>
    /// <returns>The DateTime representing the start of the specified week.</returns>
    /// <exception cref="ArgumentNullException">Thrown when culture is null.</exception>
    public static DateTime GetStartOfYearWeek(int year, int weekOfYear, CultureInfo culture,
        YearWeekType yearWeekType)
    {
        if (culture == null)
        {
            throw new ArgumentNullException("culture");
        }
        return GetStartOfYearWeek(year, weekOfYear, culture,
            culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek, yearWeekType);
    } // GetStartOfYearWeek

    /// <summary>
    /// Gets the start date of the specified week in the given year using specific week calculation rules.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="weekOfYear">The week number within the year.</param>
    /// <param name="culture">The culture to use for week calculations.</param>
    /// <param name="weekRule">The calendar week rule to use.</param>
    /// <param name="firstDayOfWeek">The first day of the week.</param>
    /// <param name="yearWeekType">The type of week calculation to use.</param>
    /// <returns>The DateTime representing the start of the specified week.</returns>
    /// <exception cref="ArgumentNullException">Thrown when culture is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when weekOfYear is less than 1.</exception>
    public static DateTime GetStartOfYearWeek(int year, int weekOfYear, CultureInfo culture,
        CalendarWeekRule weekRule, DayOfWeek firstDayOfWeek, YearWeekType yearWeekType)
    {
        if (culture == null)
        {
            throw new ArgumentNullException("culture");
        }
        if (weekOfYear < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(weekOfYear));
        }

        DateTime dateTime = new DateTime(year, 1, 1).AddDays(weekOfYear*TimeSpec.DaysPerWeek);
        int currentYear;
        int currentWeek;
        GetWeekOfYear(dateTime, culture, weekRule, firstDayOfWeek, yearWeekType, out currentYear, out currentWeek);


        // end date of week
        while (currentWeek != weekOfYear)
        {
            dateTime = dateTime.AddDays(-1);
            GetWeekOfYear(dateTime, culture, weekRule, firstDayOfWeek, yearWeekType, out currentYear,
                out currentWeek);
        }

        // end of previous week
        while (currentWeek == weekOfYear)
        {
            dateTime = dateTime.AddDays(-1);
            GetWeekOfYear(dateTime, culture, weekRule, firstDayOfWeek, yearWeekType, out currentYear,
                out currentWeek);
        }

        return dateTime.AddDays(1);
    } // GetStartOfYearWeek

    #endregion

    #region Day

    /// <summary>
    /// Gets the start of the day (midnight) for the specified DateTime.
    /// </summary>
    /// <param name="dateTime">The DateTime to get the day start for.</param>
    /// <returns>A DateTime representing the start of the day (midnight).</returns>
    public static DateTime DayStart(DateTime dateTime)
    {
        return dateTime.Date;
    } // DayStart

    /// <summary>
    /// Gets the next day of the week from the specified day.
    /// </summary>
    /// <param name="day">The current day of the week.</param>
    /// <returns>The next day of the week.</returns>
    public static DayOfWeek NextDay(DayOfWeek day)
    {
        return AddDays(day, 1);
    } // NextMonth

    /// <summary>
    /// Gets the previous day of the week from the specified day.
    /// </summary>
    /// <param name="day">The current day of the week.</param>
    /// <returns>The previous day of the week.</returns>
    public static DayOfWeek PreviousDay(DayOfWeek day)
    {
        return AddDays(day, -1);
    } // PreviousDay

    /// <summary>
    /// Adds the specified number of days to a day of the week.
    /// </summary>
    /// <param name="day">The starting day of the week.</param>
    /// <param name="days">The number of days to add (can be negative).</param>
    /// <returns>The resulting day of the week after adding the specified number of days.</returns>
    public static DayOfWeek AddDays(DayOfWeek day, int days)
    {
        if (days == 0)
        {
            return day;
        }
        int weeks = (Math.Abs(days)/TimeSpec.DaysPerWeek) + 1;

        int offset = weeks*TimeSpec.DaysPerWeek + (int) day;
        int targetOffset = offset + days;
        return (DayOfWeek) (targetOffset%TimeSpec.DaysPerWeek);
    } // AddMonths

    #endregion

}