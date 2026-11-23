namespace BloodPressure.Maui.Helpers;

public static class ApiConfig
{
    public const string HttpClientName = "BloodPressureApi";

    public static string BaseUrl
    {
        get
        {
            return "https://localhost:44319/";
        }
    }
}