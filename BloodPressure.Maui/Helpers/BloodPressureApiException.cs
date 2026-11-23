namespace BloodPressure.Maui.Helpers;

public class BloodPressureApiException : Exception
{
    public IReadOnlyList<string> Messages { get; }

    public BloodPressureApiException(IReadOnlyList<string> messages)
        : base(string.Join(" ", messages))
    {
        Messages = messages;
    }

    public BloodPressureApiException(string message)
        : base(message)
    {
        Messages = new[] { message };
    }
}