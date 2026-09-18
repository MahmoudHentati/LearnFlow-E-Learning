using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class InscriptionRepository : IInscriptionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public InscriptionRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Inscription>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Inscriptions
            .AsNoTracking()
            .Include(x => x.Formation)
            .ThenInclude(f => f!.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Inscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Inscriptions
            .AsNoTracking()
            .Include(x => x.Formation)
            .ThenInclude(f => f!.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Inscription>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            return [];
        }

        return await _dbContext.Inscriptions
            .AsNoTracking()
            .Include(x => x.Formation)
            .ThenInclude(f => f!.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .Where(x => x.EdudiantId == studentId)
            .OrderByDescending(x => x.DateInscription)
            .ToListAsync(cancellationToken);
    }

    public async Task<Inscription?> GetByStudentAndFormationAsync(string studentId, int formationId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(studentId) || formationId <= 0)
        {
            return null;
        }

        return await _dbContext.Inscriptions
            .AsNoTracking()
            .Include(x => x.Formation)
            .ThenInclude(f => f!.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .FirstOrDefaultAsync(x => x.EdudiantId == studentId && x.FormationId == formationId, cancellationToken);
    }

    public async Task<IReadOnlyList<Inscription>> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken = default)
    {
        if (formationId <= 0)
        {
            return [];
        }

        return await _dbContext.Inscriptions
            .AsNoTracking()
            .Include(x => x.Edudiant)
            .Include(x => x.Formation)
            .Where(x => x.FormationId == formationId)
            .OrderByDescending(x => x.DateInscription)
            .ToListAsync(cancellationToken);
    }

    public async Task<Inscription> CreateAsync(Inscription inscription, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inscription);
        await _dbContext.Inscriptions.AddAsync(inscription, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return inscription;
    }

    public async Task<bool> UpdateAsync(Inscription inscription, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inscription);
        var existing = await _dbContext.Inscriptions.FirstOrDefaultAsync(x => x.Id == inscription.Id, cancellationToken);
        if (existing is null) return false;

        existing.DateInscription = inscription.DateInscription;
        existing.Progression = inscription.Progression;
        existing.EdudiantId = inscription.EdudiantId;
        existing.FormationId = inscription.FormationId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Inscriptions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Inscriptions.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
