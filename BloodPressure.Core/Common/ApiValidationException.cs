using System;
using System.Collections.Generic;
using System.Text;

namespace BloodPressureApp.Core.Common
{
    public class ApiValidationException : Exception
    {
        public List<string> Errores { get; set; } = new();
        public ApiValidationException (List<string> errores)
        {
            Errores.AddRange(errores);
        }
    }
}
