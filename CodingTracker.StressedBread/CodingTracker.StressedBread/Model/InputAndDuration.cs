namespace CodingTracker.StressedBread.Model;

/// <summary>
/// Represents the dates from user input and calculated duration from it.
/// </summary>

internal class InputAndDuration
{
    public string StartDateTime { get; set; }
    public string EndDateTime { get; set; }
    public int Duration { get; set; }

    public InputAndDuration(string startDateTime, string endDateTime, int duration)
    {
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Duration = duration;
    }
}
