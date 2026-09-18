using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IInscriptionRepository
{
    Task<IReadOnlyList<Inscription>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Inscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Inscription>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<Inscription?> GetByStudentAndFormationAsync(string studentId, int formationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Inscription>> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken = default);
    Task<Inscription> CreateAsync(Inscription inscription, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Inscription inscription, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
