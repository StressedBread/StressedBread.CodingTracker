namespace CodingTracker.StressedBread;

internal class Validation
{
    public DateTime DateTimeValidation(string time)
    {
        if(DateTime.TryParse(time, out DateTime result))
        {
            return result;
        }
        else
        {
            return DateTime.MinValue;
        }
    }
}
