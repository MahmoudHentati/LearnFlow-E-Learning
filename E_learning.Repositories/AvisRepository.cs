using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class AvisRepository : IAvisRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AvisRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Avis>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Avis.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Avis?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Avis.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Avis>> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken = default)
        => await _dbContext.Avis
            .AsNoTracking()
            .Where(x => x.InscriptionId == inscriptionId)
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Avis>> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken = default)
        => await _dbContext.Avis
            .AsNoTracking()
            .Include(x => x.Inscription)
            .Where(x => x.Inscription != null && x.Inscription.FormationId == formationId)
            .OrderByDescending(x => x.Date)
            .ThenByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Avis> CreateAsync(Avis avis, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(avis);
        await _dbContext.Avis.AddAsync(avis, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return avis;
    }

    public async Task<bool> UpdateAsync(Avis avis, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(avis);
        var existing = await _dbContext.Avis.FirstOrDefaultAsync(x => x.Id == avis.Id, cancellationToken);
        if (existing is null) return false;

        existing.Note = avis.Note;
        existing.Commentaire = avis.Commentaire;
        existing.Date = avis.Date;
        existing.InscriptionId = avis.InscriptionId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Avis.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Avis.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
