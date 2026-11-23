namespace BloodPressureApp.Core.DTOs;

public class BloodPressureCreateDTO
{
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }
    public DateTime? RecordedAt { get; set; }
    public string? Notes { get; set; }
}
