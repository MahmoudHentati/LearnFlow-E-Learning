using E_Learning.Domain;
using E_learning.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace E_learning.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(roleManager);
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public Task<IReadOnlyList<AppUser>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyList<AppUser> users = _userManager.Users
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToList();

        return Task.FromResult(users);
    }

    public async Task<IReadOnlyList<AppUser>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(role))
        {
            return [];
        }

        var users = await _userManager.GetUsersInRoleAsync(role);
        return users.ToList();
    }

    public async Task<AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        return await _userManager.FindByIdAsync(id);
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(id))
        {
            return [];
        }

        var existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser is null)
        {
            return [];
        }

        var roles = await _userManager.GetRolesAsync(existingUser);
        return roles.ToList();
    }

    public async Task<(bool Success, IReadOnlyList<string> Errors, AppUser? User)> CreateAsync(
        AppUser user,
        string password,
        string role,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(password))
        {
            return (false, ["Le mot de passe est obligatoire."], null);
        }

        if (string.IsNullOrWhiteSpace(role) || !await _roleManager.RoleExistsAsync(role))
        {
            return (false, ["Le rôle demandé est invalide."], null);
        }

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return (false, createResult.Errors.Select(e => e.Description).ToList(), null);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return (false, roleResult.Errors.Select(e => e.Description).ToList(), null);
        }

        return (true, [], user);
    }

    public async Task<(bool Success, IReadOnlyList<string> Errors)> UpdateAsync(
        AppUser user,
        string role,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();

        var existingUser = await _userManager.FindByIdAsync(user.Id);
        if (existingUser is null)
        {
            return (false, ["Utilisateur introuvable."]);
        }

        if (string.IsNullOrWhiteSpace(role) || !await _roleManager.RoleExistsAsync(role))
        {
            return (false, ["Le rôle demandé est invalide."]);
        }

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.UserName = user.Email;
        existingUser.IsActive = user.IsActive;

        var updateResult = await _userManager.UpdateAsync(existingUser);
        if (!updateResult.Succeeded)
        {
            return (false, updateResult.Errors.Select(e => e.Description).ToList());
        }

        var currentRoles = await _userManager.GetRolesAsync(existingUser);
        if (currentRoles.Count > 0)
        {
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                return (false, removeRolesResult.Errors.Select(e => e.Description).ToList());
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(existingUser, role);
        if (!addRoleResult.Succeeded)
        {
            return (false, addRoleResult.Errors.Select(e => e.Description).ToList());
        }

        return (true, []);
    }

    public async Task<(bool Success, IReadOnlyList<string> Errors)> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(id))
        {
            return (false, ["Identifiant invalide."]);
        }

        var existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser is null)
        {
            return (false, ["Utilisateur introuvable."]);
        }

        var result = await _userManager.DeleteAsync(existingUser);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToList());
        }

        return (true, []);
    }
}
