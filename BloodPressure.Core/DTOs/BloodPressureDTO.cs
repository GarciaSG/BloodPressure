namespace BloodPressureApp.Core.DTOs;

public class BloodPressureDTO
{
    public int Id { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public DateTime RecordedAt { get; set; }
    public string? Notes { get; set; }
    public string Classification { get; set; } = string.Empty;
}
