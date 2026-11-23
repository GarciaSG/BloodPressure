using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BloodPressure.Maui.Helpers;
using BloodPressureApp.Core.DTOs;

namespace BloodPressure.Maui.Services;

public class BloodPressureApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BloodPressureApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient CreateClient() => _httpClientFactory.CreateClient(ApiConfig.HttpClientName);

    public async Task<List<BloodPressureDTO>> GetAllAsync()
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/bloodpressure");

            if (!response.IsSuccessStatusCode)
                throw await BuildExceptionAsync(response);

            var data = await response.Content.ReadFromJsonAsync<List<BloodPressureDTO>>();
            return data ?? new List<BloodPressureDTO>();
        }
        catch (JsonException)
        {
            throw new BloodPressureApiException("El servidor devolvió una respuesta no válida.");
        }
        catch (HttpRequestException)
        {
            throw new BloodPressureApiException("No se pudo conectar con el servidor. Verifique su conexión de red.");
        }
        catch (TaskCanceledException)
        {
            throw new BloodPressureApiException("La solicitud excedió el tiempo de espera. Intente nuevamente.");
        }
    }

    public async Task<BloodPressureDTO?> GetByIdAsync(int id)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/bloodpressure/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
                throw await BuildExceptionAsync(response);

            return await response.Content.ReadFromJsonAsync<BloodPressureDTO>();
        }
        catch (JsonException)
        {
            throw new BloodPressureApiException("El servidor devolvió una respuesta no válida.");
        }
        catch (HttpRequestException)
        {
            throw new BloodPressureApiException("No se pudo conectar con el servidor. Verifique su conexión de red.");
        }
        catch (TaskCanceledException)
        {
            throw new BloodPressureApiException("La solicitud excedió el tiempo de espera. Intente nuevamente.");
        }
    }

    public async Task<BloodPressureDTO> CreateAsync(BloodPressureCreateDTO dto)
    {
        try
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("api/bloodpressure", dto);

            if (!response.IsSuccessStatusCode)
                throw await BuildExceptionAsync(response);

            var created = await response.Content.ReadFromJsonAsync<BloodPressureDTO>();
            return created ?? throw new BloodPressureApiException("Respuesta inesperada del servidor.");
        }
        catch (JsonException)
        {
            throw new BloodPressureApiException("El servidor devolvió una respuesta no válida.");
        }
        catch (HttpRequestException)
        {
            throw new BloodPressureApiException("No se pudo conectar con el servidor. Verifique su conexión de red.");
        }
        catch (TaskCanceledException)
        {
            throw new BloodPressureApiException("La solicitud excedió el tiempo de espera. Intente nuevamente.");
        }
    }

    public async Task<BloodPressureDTO> UpdateAsync(int id, BloodPressureUpdateDTO dto)
    {
        try
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/bloodpressure/{id}", dto);

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new BloodPressureApiException("La medición que intenta actualizar ya no existe.");

            if (!response.IsSuccessStatusCode)
                throw await BuildExceptionAsync(response);

            var updated = await response.Content.ReadFromJsonAsync<BloodPressureDTO>();
            return updated ?? throw new BloodPressureApiException("Respuesta inesperada del servidor.");
        }
        catch (JsonException)
        {
            throw new BloodPressureApiException("El servidor devolvió una respuesta no válida.");
        }
        catch (HttpRequestException)
        {
            throw new BloodPressureApiException("No se pudo conectar con el servidor. Verifique su conexión de red.");
        }
        catch (TaskCanceledException)
        {
            throw new BloodPressureApiException("La solicitud excedió el tiempo de espera. Intente nuevamente.");
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/bloodpressure/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new BloodPressureApiException("La medición que intenta eliminar ya no existe.");

            if (!response.IsSuccessStatusCode)
                throw await BuildExceptionAsync(response);
        }
        catch (HttpRequestException)
        {
            throw new BloodPressureApiException("No se pudo conectar con el servidor. Verifique su conexión de red.");
        }
        catch (TaskCanceledException)
        {
            throw new BloodPressureApiException("La solicitud excedió el tiempo de espera. Intente nuevamente.");
        }
    }

    private static async Task<BloodPressureApiException> BuildExceptionAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            try
            {
                var messages = await response.Content.ReadFromJsonAsync<List<string>>();
                if (messages is { Count: > 0 })
                    return new BloodPressureApiException(messages);
            }
            catch
            {
            }

            return new BloodPressureApiException("Los datos enviados no son válidos.");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
            return new BloodPressureApiException("El recurso solicitado no existe.");

        if ((int)response.StatusCode >= 500)
            return new BloodPressureApiException("Ocurrió un error en el servidor. Intente nuevamente más tarde.");

        return new BloodPressureApiException("No se pudo completar la operación. Intente nuevamente.");
    }
}