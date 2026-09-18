using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VideoRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Video>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Videos.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Video?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Videos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Video> CreateAsync(Video video, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(video);
        await _dbContext.Videos.AddAsync(video, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return video;
    }

    public async Task<bool> UpdateAsync(Video video, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(video);
        var existing = await _dbContext.Videos.FirstOrDefaultAsync(x => x.Id == video.Id, cancellationToken);
        if (existing is null) return false;

        existing.Titre = video.Titre;
        existing.Ordre = video.Ordre;
        existing.UrlVideo = video.UrlVideo;
        existing.Duree = video.Duree;
        existing.ModuleId = video.ModuleId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Videos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Videos.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
