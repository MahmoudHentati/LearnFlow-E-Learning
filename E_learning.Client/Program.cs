using E_learning.Client;
using E_learning.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.HostEnvironment.IsDevelopment()
    ? "https://localhost:7202/"
    : builder.HostEnvironment.BaseAddress;

builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddScoped<ClientThemeService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRoleBasedRedirectService, RoleBasedRedirectService>();

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
