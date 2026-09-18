using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Document>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Documents.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Document?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Documents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Document> CreateAsync(Document document, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        await _dbContext.Documents.AddAsync(document, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return document;
    }

    public async Task<bool> UpdateAsync(Document document, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);
        var existing = await _dbContext.Documents.FirstOrDefaultAsync(x => x.Id == document.Id, cancellationToken);
        if (existing is null) return false;

        existing.Titre = document.Titre;
        existing.Ordre = document.Ordre;
        existing.UrlFichier = document.UrlFichier;
        existing.Format = document.Format;
        existing.ModuleId = document.ModuleId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Documents.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Documents.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
