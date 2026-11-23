using BloodPressureApp.Core.DTOs;
using FluentValidation;

namespace BloodPressureApp.Core.Validators;

public abstract class BloodPressureValidatorBase<T> : AbstractValidator<T>
    where T : BloodPressureCreateDTO
{
    protected BloodPressureValidatorBase()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Systolic)
            .NotNull().WithMessage("La presión sistólica es obligatoria.")
            .InclusiveBetween(60, 250).WithMessage("La presión sistólica debe estar entre 60 y 250 mmHg.");

        RuleFor(x => x.Diastolic)
            .NotNull().WithMessage("La presión diastólica es obligatoria.")
            .InclusiveBetween(40, 130).WithMessage("La presión diastólica debe estar entre 40 y 130 mmHg.")
            .Must((dto, diastolic) => !dto.Systolic.HasValue || diastolic < dto.Systolic)
            .WithMessage("La presión diastólica debe ser menor que la sistólica.");

        RuleFor(x => x.RecordedAt)
            .Must(fecha => fecha.HasValue && fecha.Value != default)
            .WithMessage("La fecha y hora de la medición no son válidas.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Las notas no pueden superar los 500 caracteres.");
    }
}
