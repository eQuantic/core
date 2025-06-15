namespace eQuantic.Core.Date;

/// <summary>
/// Specifies the format type for duration display.
/// </summary>
public enum DurationFormatType
{
    /// <summary>
    /// Compact format for duration display.
    /// </summary>
    Compact,
    
    /// <summary>
    /// Detailed format for duration display.
    /// </summary>
    Detailed,
}

/// <summary>
/// Specifies the edge type for intervals.
/// </summary>
public enum IntervalEdge
{
    /// <summary>
    /// Closed interval edge (inclusive).
    /// </summary>
    Closed,
    
    /// <summary>
    /// Open interval edge (exclusive).
    /// </summary>
    Open,
}

/// <summary>
/// Represents the half-year periods within a year.
/// </summary>
public enum YearHalfyear
{
    /// <summary>
    /// First half of the year (January to June).
    /// </summary>
    First = 1,
    
    /// <summary>
    /// Second half of the year (July to December).
    /// </summary>
    Second = 2,
}

/// <summary>
/// Represents the months of the year.
/// </summary>
public enum YearMonth
{
    /// <summary>
    /// January (month 1).
    /// </summary>
    January = 1,
    
    /// <summary>
    /// February (month 2).
    /// </summary>
    February = 2,
    
    /// <summary>
    /// March (month 3).
    /// </summary>
    March = 3,
    
    /// <summary>
    /// April (month 4).
    /// </summary>
    April = 4,
    
    /// <summary>
    /// May (month 5).
    /// </summary>
    May = 5,
    
    /// <summary>
    /// June (month 6).
    /// </summary>
    June = 6,
    
    /// <summary>
    /// July (month 7).
    /// </summary>
    July = 7,
    
    /// <summary>
    /// August (month 8).
    /// </summary>
    August = 8,
    
    /// <summary>
    /// September (month 9).
    /// </summary>
    September = 9,
    
    /// <summary>
    /// October (month 10).
    /// </summary>
    October = 10,
    
    /// <summary>
    /// November (month 11).
    /// </summary>
    November = 11,
    
    /// <summary>
    /// December (month 12).
    /// </summary>
    December = 12
}

/// <summary>
/// Represents the quarters within a year.
/// </summary>
public enum YearQuarter
{
    /// <summary>
    /// First quarter (January to March).
    /// </summary>
    First = 1,
    
    /// <summary>
    /// Second quarter (April to June).
    /// </summary>
    Second = 2,
    
    /// <summary>
    /// Third quarter (July to September).
    /// </summary>
    Third = 3,
    
    /// <summary>
    /// Fourth quarter (October to December).
    /// </summary>
    Fourth = 4,
}

/// <summary>
/// Specifies the type of week numbering system to use.
/// </summary>
public enum YearWeekType
{
    /// <summary>
    /// Calendar week numbering system.
    /// </summary>
    Calendar,
    
    /// <summary>
    /// ISO 8601 week numbering system.
    /// </summary>
    Iso8601,
}