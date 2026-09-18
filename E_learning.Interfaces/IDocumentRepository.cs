using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IDocumentRepository
{
    Task<IReadOnlyList<Document>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Document?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Document> CreateAsync(Document document, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Document document, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
