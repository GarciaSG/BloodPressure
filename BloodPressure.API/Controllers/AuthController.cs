using BloodPressureApp.API.Services;
using BloodPressureApp.Core.DTOs;
using BloodPressureApp.Core.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodPressureApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService userService;

        public AuthController(UserService userService, LoginValidator validationRules)
        {
            this.userService = userService;
            ValidationRules = validationRules;
        }

        public LoginValidator ValidationRules { get; }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dTO)
        {
            var validationresult = ValidationRules.Validate(dTO);
            if (validationresult.IsValid)
            {
                var token = await userService.Login(dTO);

                if (token is null)
                    return BadRequest(new[] { "El usuaio o la contraseña son incorrectos" });

                return Ok(token);
            }
            else
            {
                return BadRequest(validationresult.Errors.Select(x => x.ErrorMessage));
            }
        }
    }
}
