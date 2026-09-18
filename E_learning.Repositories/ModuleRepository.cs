using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class ModuleRepository : IModuleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ModuleRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Module>> GetAllAsync(CancellationToken cancellationToken = default)
        => await QueryModules()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Module?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await QueryModules()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Module>> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken = default)
        => await QueryModules()
            .Where(x => x.FormationId == formationId)
            .OrderBy(x => x.Ordre)
            .ToListAsync(cancellationToken);

    public async Task<Module> CreateAsync(Module module, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(module);
        await _dbContext.Modules.AddAsync(module, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return module;
    }

    public async Task<bool> UpdateAsync(Module module, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(module);
        var existing = await _dbContext.Modules.FirstOrDefaultAsync(x => x.Id == module.Id, cancellationToken);
        if (existing is null) return false;

        existing.Titre = module.Titre;
        existing.Ordre = module.Ordre;
        existing.FormationId = module.FormationId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Modules.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Modules.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private IQueryable<Module> QueryModules()
        => _dbContext.Modules
            .AsNoTracking()
            .Include(x => x.Formation)
            .Include(x => x.Videos)
            .Include(x => x.Documents)
            .Include(x => x.Quizzes)
            .ThenInclude(q => q.Questions)
            .ThenInclude(question => question.OptionsReponses)
            .Include(x => x.Tests);
}
