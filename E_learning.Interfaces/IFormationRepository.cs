using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IFormationRepository
{
    /// <summary>
    /// Retourne toutes les formations.
    /// </summary>
    Task<IReadOnlyList<Formation>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retourne une formation par identifiant.
    /// </summary>
    Task<Formation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crée une nouvelle formation.
    /// </summary>
    Task<Formation> CreateAsync(Formation formation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Met à jour une formation existante.
    /// </summary>
    Task<bool> UpdateAsync(Formation formation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Supprime une formation par identifiant.
    /// </summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retourne toutes les formations créées par un formateur spécifique.
    /// </summary>
    Task<IReadOnlyList<Formation>> GetByFormateurIdAsync(string formateurId, CancellationToken cancellationToken = default);
}
