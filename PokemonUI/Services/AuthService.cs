using System.Net.Http.Json;
using PokemonUI.Models.Auth;

public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("PokemonAPI");
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/login", request);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/register", request);

        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (!response.IsSuccessStatusCode)
            return (false, content?.Message ?? "Error desconocido");

        return (true, content!.Message);
    }
}

