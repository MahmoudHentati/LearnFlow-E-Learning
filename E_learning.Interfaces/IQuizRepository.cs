using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IQuizRepository
{
    Task<IReadOnlyList<Quiz>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Quiz?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Quiz>> GetByModuleIdAsync(int moduleId, CancellationToken cancellationToken = default);
    Task<Quiz> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Quiz quiz, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
