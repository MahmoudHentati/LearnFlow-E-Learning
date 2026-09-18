using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IParticipationQuizRepository
{
    Task<IReadOnlyList<ParticipationQuiz>> GetAllAsync(CancellationToken cancellationToken);
    Task<ParticipationQuiz?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ParticipationQuiz> CreateAsync(ParticipationQuiz participationQuiz, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(ParticipationQuiz participationQuiz, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ParticipationQuiz>> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken);
    Task<IReadOnlyList<ParticipationQuiz>> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken);
}