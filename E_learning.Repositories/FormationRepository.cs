using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class FormationRepository : IFormationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FormationRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Formation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Formations
            .AsNoTracking()
            .Include(f => f.Formateur)
            .Include(f => f.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Videos)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Documents)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Quizzes)
            .ThenInclude(q => q.Questions)
            .ThenInclude(question => question.OptionsReponses)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Quizzes)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Tests)
            .OrderBy(f => f.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Formation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Formations
            .AsNoTracking()
            .Include(f => f.Formateur)
            .Include(f => f.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Videos)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Documents)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Quizzes)
            .ThenInclude(q => q.Questions)
            .ThenInclude(question => question.OptionsReponses)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Quizzes)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Tests)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<Formation> CreateAsync(Formation formation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(formation);

        await _dbContext.Formations.AddAsync(formation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return formation;
    }

    public async Task<bool> UpdateAsync(Formation formation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(formation);

        var existingFormation = await _dbContext.Formations.FirstOrDefaultAsync(f => f.Id == formation.Id, cancellationToken);
        if (existingFormation is null)
        {
            return false;
        }

        existingFormation.Titre = formation.Titre;
        existingFormation.Description = formation.Description;
        existingFormation.Prerequis = formation.Prerequis;
        existingFormation.Difficulte = formation.Difficulte;
        existingFormation.DateCreation = formation.DateCreation;
        existingFormation.Statut = formation.Statut;
        existingFormation.FormateurId = formation.FormateurId;
        existingFormation.SousCategorieId = formation.SousCategorieId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var formation = await _dbContext.Formations.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        if (formation is null)
        {
            return false;
        }

        _dbContext.Formations.Remove(formation);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<Formation>> GetByFormateurIdAsync(string formateurId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(formateurId))
        {
            return [];
        }

        return await _dbContext.Formations
            .AsNoTracking()
            .Where(f => f.FormateurId == formateurId)
            .Include(f => f.Formateur)
            .Include(f => f.SousCategorie)
            .ThenInclude(sc => sc!.Categorie)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Videos)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Documents)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Quizzes)
            .ThenInclude(q => q.Questions)
            .ThenInclude(question => question.OptionsReponses)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Quizzes)
            .Include(f => f.Modules)
            .ThenInclude(m => m.Tests)
            .OrderByDescending(f => f.DateCreation)
            .ToListAsync(cancellationToken);
    }
}
