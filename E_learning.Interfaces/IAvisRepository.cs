using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IAvisRepository
{
    Task<IReadOnlyList<Avis>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Avis?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Avis>> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Avis>> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken = default);
    Task<Avis> CreateAsync(Avis avis, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Avis avis, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
