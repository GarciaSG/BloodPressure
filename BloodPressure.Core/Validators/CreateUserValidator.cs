using BloodPressureApp.Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodPressureApp.Core.Validators
{
    public class CreateUserValidator : AbstractValidator<UserCreateDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("El nombre de usuario es obligatorio").MaximumLength(25).WithMessage("El nombre de usuario debe tener un maximo de 25 caracteres");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("La contraseña es obligatoria").MinimumLength(8).MaximumLength(12).WithMessage("La contraseña debe tener un minimo de 8 y un maximo de 12 caracteres");

            RuleFor(x => x).Must(MustBeEqual).WithMessage("Las contraseñas no coinciden");


        }

        bool MustBeEqual(UserCreateDTO dto)
        {
            if (dto.ConfirmPassword is null)
            {
                return false;
            }

            return dto.ConfirmPassword.Equals(dto.Password);
        }
    }
}
