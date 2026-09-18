using E_Learning.Domain;
using E_learning.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Learning.API.Services;

public static class AuthSeedService
{
    /// <summary>
    /// Seeds application roles and default users used for authentication in development.
    /// </summary>
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        var roles = new[]
        {
            AppRoles.SuperAdmin,
            AppRoles.Admin,
            AppRoles.Student,
            AppRoles.Instructor
        };

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Role seed failed for '{role}': {errors}");
            }
        }

        var adminEmail = configuration["AuthSeed:AdminEmail"];
        var adminPassword = configuration["AuthSeed:AdminPassword"];
        var instructorEmail = configuration["AuthSeed:InstructorEmail"];
        var instructorPassword = configuration["AuthSeed:InstructorPassword"];
        var studentEmail = configuration["AuthSeed:StudentEmail"];
        var studentPassword = configuration["AuthSeed:StudentPassword"];

        var seedUsers = new[]
        {
            new SeedUser(adminEmail, adminPassword, "System", "Admin", AppRoles.Admin),
            new SeedUser(instructorEmail, instructorPassword, "Default", "Instructor", AppRoles.Instructor),
            new SeedUser(studentEmail, studentPassword, "Default", "Student", AppRoles.Student)
        };

        foreach (var seedUser in seedUsers)
        {
            if (string.IsNullOrWhiteSpace(seedUser.Email) || string.IsNullOrWhiteSpace(seedUser.Password))
            {
                continue;
            }

            var appUser = await userManager.FindByEmailAsync(seedUser.Email);
            if (appUser == null)
            {
                appUser = new AppUser
                {
                    UserName = seedUser.Email,
                    Email = seedUser.Email,
                    EmailConfirmed = true,
                    FirstName = seedUser.FirstName,
                    LastName = seedUser.LastName,
                    IsActive = true
                };

                var createResult = await userManager.CreateAsync(appUser, seedUser.Password);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"User seed failed for '{seedUser.Email}': {errors}");
                }
            }

            if (await userManager.IsInRoleAsync(appUser, seedUser.Role))
            {
                continue;
            }

            var addRoleResult = await userManager.AddToRoleAsync(appUser, seedUser.Role);
            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Add role '{seedUser.Role}' failed for '{seedUser.Email}': {errors}");
            }
        }
    }

    private sealed record SeedUser(string? Email, string? Password, string FirstName, string LastName, string Role);
}