using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class SousCategorieRepository : ISousCategorieRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SousCategorieRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<SousCategorie>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SousCategories
            .AsNoTracking()
            .Include(x => x.Categorie)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<SousCategorie?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.SousCategories
            .AsNoTracking()
            .Include(x => x.Categorie)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<SousCategorie> CreateAsync(SousCategorie sousCategorie, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sousCategorie);
        await _dbContext.SousCategories.AddAsync(sousCategorie, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return sousCategorie;
    }

    public async Task<bool> UpdateAsync(SousCategorie sousCategorie, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sousCategorie);
        var existing = await _dbContext.SousCategories.FirstOrDefaultAsync(x => x.Id == sousCategorie.Id, cancellationToken);
        if (existing is null) return false;

        existing.Nom = sousCategorie.Nom;
        existing.Description = sousCategorie.Description;
        existing.CategorieId = sousCategorie.CategorieId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.SousCategories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.SousCategories.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
