using System;
using System.Collections.Generic;

namespace BloodPressureApp.API.Models;

public partial class Users
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<BloodPressure> Bloodpressures { get; set; } = new List<BloodPressure>();
}
