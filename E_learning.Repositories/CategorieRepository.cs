using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class CategorieRepository : ICategorieRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategorieRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Categorie>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Categories.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task<Categorie?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Categorie> CreateAsync(Categorie categorie, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categorie);
        await _dbContext.Categories.AddAsync(categorie, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return categorie;
    }

    public async Task<bool> UpdateAsync(Categorie categorie, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categorie);
        var existing = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == categorie.Id, cancellationToken);
        if (existing is null) return false;

        existing.Nom = categorie.Nom;
        existing.Description = categorie.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Categories.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
