using BloodPressureApp.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BloodPressureApp.Core.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Escriba el nombre de usuario");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Escriba la contraseña");

        }
    }
}
