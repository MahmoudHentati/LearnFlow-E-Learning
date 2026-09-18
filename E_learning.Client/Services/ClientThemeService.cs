namespace E_learning.Client.Services;

public sealed class ClientThemeService
{
    private const string ThemeStorageKey = "ui-theme-mode";
    private readonly ITokenStorageService _tokenStorageService;

    public ClientThemeService(ITokenStorageService tokenStorageService)
    {
        ArgumentNullException.ThrowIfNull(tokenStorageService);
        _tokenStorageService = tokenStorageService;
    }

    public bool IsDarkMode { get; private set; } = true;

    public event Action? ThemeChanged;

    public async Task InitializeAsync()
    {
        var persistedTheme = await _tokenStorageService.GetItemAsync<string>(ThemeStorageKey);
        if (string.Equals(persistedTheme, "light", StringComparison.OrdinalIgnoreCase))
        {
            IsDarkMode = false;
            return;
        }

        IsDarkMode = true;
    }

    public async Task ToggleAsync()
    {
        IsDarkMode = !IsDarkMode;
        await PersistAsync();
        ThemeChanged?.Invoke();
    }

    private async Task PersistAsync()
    {
        var themeValue = IsDarkMode ? "dark" : "light";
        await _tokenStorageService.SetItemAsync(ThemeStorageKey, themeValue);
    }
}
