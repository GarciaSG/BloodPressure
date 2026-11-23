using BloodPressure.Maui.Helpers;
using BloodPressureApp.Core.Common;
using BloodPressureApp.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace BloodPressure.Maui.Services
{
    public class UserService
    {
        private readonly HttpClient client;

        public UserService(IHttpClientFactory factory)
        {
            client = factory.CreateClient(ApiConfig.HttpClientName);
        }

        public async Task<UserResponseDTO?> Register(UserCreateDTO dTO)
        {
            var response = await client.PostAsJsonAsync<UserCreateDTO>("api/users", dTO);

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserResponseDTO>();
                return user;
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errores = await response.Content.ReadFromJsonAsync<List<string>>();
                throw new ApiValidationException(errores ?? ["Ocurrio un error..."]);
            }
            else
            {
                var errores = await response.Content.ReadAsStringAsync();
                throw new ApiValidationException([errores]);
            }
        }
    }
}
