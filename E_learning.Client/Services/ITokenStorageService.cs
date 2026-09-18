namespace E_learning.Client.Services;

public interface ITokenStorageService
{
    Task SetItemAsync<T>(string key, T value);
    Task<T?> GetItemAsync<T>(string key);
    Task RemoveItemAsync(string key);
}