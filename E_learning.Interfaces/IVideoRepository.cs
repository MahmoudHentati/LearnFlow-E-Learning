using E_Learning.Domain.Entities;

namespace E_learning.Interfaces;

public interface IVideoRepository
{
    Task<IReadOnlyList<Video>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Video?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Video> CreateAsync(Video video, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Video video, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
