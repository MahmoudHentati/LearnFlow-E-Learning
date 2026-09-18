using E_Learning.Domain;

namespace E_learning.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Retourne tous les utilisateurs.
    /// </summary>
    Task<IReadOnlyList<AppUser>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retourne les utilisateurs du rôle demandé.
    /// </summary>
    Task<IReadOnlyList<AppUser>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retourne un utilisateur par son identifiant.
    /// </summary>
    Task<AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retourne les rôles d'un utilisateur.
    /// </summary>
    Task<IReadOnlyList<string>> GetRolesAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crée un utilisateur et l'assigne au rôle demandé.
    /// </summary>
    Task<(bool Success, IReadOnlyList<string> Errors, AppUser? User)> CreateAsync(
        AppUser user,
        string password,
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Met à jour un utilisateur et son rôle.
    /// </summary>
    Task<(bool Success, IReadOnlyList<string> Errors)> UpdateAsync(
        AppUser user,
        string role,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Supprime un utilisateur.
    /// </summary>
    Task<(bool Success, IReadOnlyList<string> Errors)> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
