using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface ICategorieRepository
{
    Task<IReadOnlyList<Categorie>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Categorie?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Categorie> CreateAsync(Categorie categorie, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Categorie categorie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
