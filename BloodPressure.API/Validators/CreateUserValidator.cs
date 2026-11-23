using BloodPressureApp.API.Repositories;
using FluentValidation;

namespace BloodPressureApp.API.Validators
{
    public class CreateUserValidator:Core.Validators.CreateUserValidator
    {
        private readonly UserRepository repository;

        public CreateUserValidator(UserRepository repository) :base()
        {
            this.repository = repository;

            RuleFor(x => x.Username).Must(VerfificarRepeticion).WithMessage("el nombre de usuario ya existe");
        }

        private bool VerfificarRepeticion(string? username)
        {
            return !repository.UserExists(username ?? "");
        }
    }
}
