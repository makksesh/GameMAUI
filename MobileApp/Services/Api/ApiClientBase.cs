using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api;

public abstract class ApiClientBase
{
    protected readonly HttpClient Http;
    private readonly ITokenStorage _tokenStorage;

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected ApiClientBase(IHttpClientFactory factory, ITokenStorage tokenStorage)
    {
        Http = factory.CreateClient("GameRpgApi");
        _tokenStorage = tokenStorage;
    }
    
    protected async Task AuthorizeAsync()
    {
        var token = await _tokenStorage.GetAsync();
        if (!string.IsNullOrWhiteSpace(token))
            Http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }
    
    protected async Task<T?> GetAsync<T>(string url)
    {
        await AuthorizeAsync();
        var response = await Http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
    
    protected async Task<T?> PostAsync<T>(string url, object? body = null)
    {
        await AuthorizeAsync();
        var content  = body is null ? null : Serialize(body);
        var response = await Http.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
    
    protected async Task PostAsync(string url, object? body = null)
    {
        await AuthorizeAsync();
        var content  = body is null ? null : Serialize(body);
        var response = await Http.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }
    
    protected async Task<T?> PatchAsync<T>(string url, object body)
    {
        await AuthorizeAsync();
        var request  = new HttpRequestMessage(HttpMethod.Patch, url) { Content = Serialize(body) };
        var response = await Http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
    
    protected async Task DeleteAsync(string url)
    {
        await AuthorizeAsync();
        var response = await Http.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    protected static StringContent Serialize(object body) =>
        new(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
}