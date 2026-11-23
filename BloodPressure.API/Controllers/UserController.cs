using BloodPressureApp.API.Services;
using BloodPressureApp.API.Validators;
using BloodPressureApp.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodPressureApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CreateUserValidator validationRules;

        public UserController(UserService userService, CreateUserValidator validationRules)
        {
            UserService = userService;
            this.validationRules = validationRules;
        }

        public UserService UserService { get; }

        [HttpPost]
        public async Task<IActionResult> Post(UserCreateDTO dTO)
        {
            var result = validationRules.Validate(dTO);
            if (result.IsValid)
            {
                var user =await UserService.Register(dTO);
                return Ok(user);
            }
            else
            {
                return BadRequest(result.Errors.Select(x => x.ErrorMessage));
            }
        }
    }
}
