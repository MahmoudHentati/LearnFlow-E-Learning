using System.Text.Json;
using Microsoft.JSInterop;

namespace E_learning.Client.Services;

public class TokenStorageService : ITokenStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public TokenStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var jsonValue = JsonSerializer.Serialize(value);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, jsonValue);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var jsonValue = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        if (string.IsNullOrWhiteSpace(jsonValue))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonValue);
    }

    public async Task RemoveItemAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}