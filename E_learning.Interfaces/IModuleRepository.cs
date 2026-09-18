using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IModuleRepository
{
    Task<IReadOnlyList<Module>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Module?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Module>> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken = default);
    Task<Module> CreateAsync(Module module, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Module module, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
