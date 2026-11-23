using BloodPressureApp.API.Mappers;
using BloodPressureApp.API.Models;
using BloodPressureApp.API.Repositories;
using BloodPressureApp.Core.DTOs;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BloodPressureApp.API.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;
    

        public IConfiguration Configuration { get; }

        public UserService(UserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            Configuration = configuration;
        }

        public async Task<UserResponseDTO> Register (UserCreateDTO dTO)
        {
            //hash de password
            dTO.Password = BCrypt.Net.BCrypt.HashPassword(dTO.Password);

            var entity = dTO.ToEntity();
            var result = await userRepository.AddAsync(entity);

            return result.ToDTO();
        }

        public async Task<string?> Login(LoginDTO dTO)
        {
            //dTO.Password = BCrypt.Net.BCrypt.HashPassword(dTO.Password);
            var user = userRepository.GetUser(dTO.Username);

            if (user is null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dTO.Password, user.Password)) return null;

            var jwt = Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwt["SecretKey"]!);

            //generar las creddenciasles para firmar el token
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);


            //create los claims 
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,
                          user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
