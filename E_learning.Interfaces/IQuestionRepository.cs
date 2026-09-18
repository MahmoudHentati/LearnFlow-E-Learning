using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IQuestionRepository
{
    Task<IReadOnlyList<Question>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Question>> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken = default);
    Task<Question> CreateAsync(Question question, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Question question, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
