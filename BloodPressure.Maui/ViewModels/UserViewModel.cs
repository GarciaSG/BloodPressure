using BloodPressure.Maui.Services;
using BloodPressureApp.Core.DTOs;
using BloodPressureApp.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BloodPressure.Maui.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private UserCreateDTO usuarioRegistar = null!;
        private string error = string.Empty;
        private readonly CreateUserValidator validatior;
        private readonly UserService service;

        public string Error { get => error; set => SetProperty(ref error, value); }


        public UserCreateDTO UsuarioRegistar { get => usuarioRegistar; set => SetProperty(ref usuarioRegistar, value); }
        public ICommand GotoRegisterCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public UserViewModel(CreateUserValidator validatior, UserService service)
        {
            GotoRegisterCommand = new Command(GotoRegister);
            CancelarCommand = new Command(Cancel);
            RegisterCommand = new Command(Register);
            this.validatior = validatior;
            this.service = service;
        }

        private async void Register()
        {
            Error = "";
            if (usuarioRegistar is null)
                return;
            
            var result = validatior.Validate(usuarioRegistar);
            try
            {
                if (result.IsValid)
                {
                    
                    var user = await service.Register(usuarioRegistar);
                    if (user != null)
                    {
                        await Shell.Current.DisplayAlertAsync("Usuario registrado", $"El usuario {user.Username} ha sido registrado", "OK");
                        await Shell.Current.GoToAsync("..");
                    }
                }
                else
                {
                    Error = string.Join("/n", result.Errors.Select(x => "■ " + x.ErrorMessage));
                }
            }
            catch
            {
                Error = string.Join("/n", result.Errors.Select(x => "■ " + x.ErrorMessage));
            }

        }

        private async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void GotoRegister()
        {
            Error = string.Empty;
            usuarioRegistar = new();
            await Shell.Current.GoToAsync("register");
        }
    }
}
