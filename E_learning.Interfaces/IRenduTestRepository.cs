using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IRenduTestRepository
{
    Task<IReadOnlyList<RenduTest>> GetAllAsync(CancellationToken cancellationToken);
    Task<RenduTest?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<RenduTest>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken);
    Task<RenduTest> CreateAsync(RenduTest renduTest, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(RenduTest renduTest, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<RenduTest>> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RenduTest>> GetByTestIdAsync(int testId, CancellationToken cancellationToken);
}
