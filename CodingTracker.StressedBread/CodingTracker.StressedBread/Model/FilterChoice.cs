using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.Model;

/// <summary>
/// Represents the data of picked Filter Period and Filter Type.
/// </summary>

internal class FilterChoice
{
    public FilterPeriod FilterPeriod { get; set; }
    public FilterType FilterType { get; set; }

    public FilterChoice(FilterPeriod filterPeriod, FilterType filterType)
    {
        FilterPeriod = filterPeriod;
        FilterType = filterType;
    }
}
