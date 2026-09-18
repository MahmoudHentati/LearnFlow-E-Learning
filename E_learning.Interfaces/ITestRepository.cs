using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface ITestRepository
{
    Task<IReadOnlyList<Test>> GetAllAsync(CancellationToken cancellationToken);
    Task<Test?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Test>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken);
    Task<Test> CreateAsync(Test test, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Test test, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Test>> GetByModuleIdAsync(int moduleId, CancellationToken cancellationToken);
}
