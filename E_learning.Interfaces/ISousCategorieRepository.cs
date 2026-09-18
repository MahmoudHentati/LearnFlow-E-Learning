using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface ISousCategorieRepository
{
    Task<IReadOnlyList<SousCategorie>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SousCategorie?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SousCategorie> CreateAsync(SousCategorie sousCategorie, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(SousCategorie sousCategorie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
