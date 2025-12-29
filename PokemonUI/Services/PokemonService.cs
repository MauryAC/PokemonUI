using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using PokemonUI.Models.Pokemon;

public class PokemonService
{
    private readonly HttpClient _http;
    private readonly AuthState _authState;

    public PokemonService(IHttpClientFactory factory, AuthState authState)
    {
        _http = factory.CreateClient("PokemonAPI");
        _authState = authState;
    }

    public async Task<PokemonResponse?> GetPokemonByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var token = _authState.Token;

        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException("Usuario no autenticado");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.GetAsync($"api/pokemon/{name}");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException("Token inválido o expirado");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error al consultar el Pokémon");

        return await response.Content.ReadFromJsonAsync<PokemonResponse>();
    }

    public async Task<List<PokemonResponse>> GetAllPokemonsAsync(int limit = 20, int offset = 0)
    {
        var token = _authState.Token;
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException();

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.GetAsync(
            $"api/pokemon?limit={limit}&offset={offset}"
        );

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();

        if (!response.IsSuccessStatusCode)
            return new List<PokemonResponse>();

        return await response.Content.ReadFromJsonAsync<List<PokemonResponse>>()
               ?? new List<PokemonResponse>();
    }
}
