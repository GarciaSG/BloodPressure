using System;
using System.Collections.Generic;
using System.Text;

namespace BloodPressureApp.Core.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
    }
}
