using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IOptionReponseRepository
{
    Task<IReadOnlyList<OptionReponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<OptionReponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<OptionReponse> CreateAsync(OptionReponse optionReponse, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(OptionReponse optionReponse, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<OptionReponse>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken);
}