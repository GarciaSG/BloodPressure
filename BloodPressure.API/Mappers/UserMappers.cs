using BloodPressureApp.API.Models;
using BloodPressureApp.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace BloodPressureApp.API.Mappers 
{
    public static class UserMappers
    {
        public static UserResponseDTO ToDTO(this Users entity)
        {
            return new UserResponseDTO
            {
                Id = entity.Id,
                Username = entity.Username
            };
        }

        public static Users ToEntity (this UserCreateDTO dto)
        {
            return new Users
            {
                Username = dto.Username ?? "",
                Password = dto.Password ?? ""
            };
        }
    }
}
