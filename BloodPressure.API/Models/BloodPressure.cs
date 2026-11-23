using System;
using System.Collections.Generic;

namespace BloodPressureApp.API.Models;

public partial class BloodPressure
{
    public int Id { get; set; }

    public int Systolic { get; set; }

    public int Diastolic { get; set; }

    public DateTime RecordedAt { get; set; }

    public string? Notes { get; set; }

    public int? UserId { get; set; }

    public virtual Users? User { get; set; }
}
