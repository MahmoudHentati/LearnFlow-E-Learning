using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class RenduTestRepository : IRenduTestRepository
{
    private readonly ApplicationDbContext _context;

    public RenduTestRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task<IReadOnlyList<RenduTest>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await QueryRendus()
            .ToListAsync(cancellationToken);
    }

    public async Task<RenduTest?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await QueryRendus()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<RenduTest>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(instructorId))
        {
            return [];
        }

        return await QueryRendus()
            .Where(r => r.Test != null &&
                        r.Test.Module != null &&
                        r.Test.Module.Formation != null &&
                        r.Test.Module.Formation.FormateurId == instructorId)
            .OrderByDescending(r => r.DateSoumission)
            .ToListAsync(cancellationToken);
    }

    public async Task<RenduTest> CreateAsync(RenduTest renduTest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(renduTest);

        _context.RenduTests.Add(renduTest);
        await _context.SaveChangesAsync(cancellationToken);
        return renduTest;
    }

    public async Task<bool> UpdateAsync(RenduTest renduTest, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(renduTest);

        var existing = await _context.RenduTests.FirstOrDefaultAsync(r => r.Id == renduTest.Id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.DateSoumission = renduTest.DateSoumission;
        existing.Contenu = renduTest.Contenu;
        existing.FichierUrl = renduTest.FichierUrl;
        existing.Note = renduTest.Note;
        existing.CommentaireFormateur = renduTest.CommentaireFormateur;
        existing.TestId = renduTest.TestId;
        existing.InscriptionId = renduTest.InscriptionId;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var renduTest = await _context.RenduTests.FindAsync(new object[] { id }, cancellationToken);
        if (renduTest is null)
        {
            return false;
        }

        _context.RenduTests.Remove(renduTest);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IReadOnlyList<RenduTest>> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken)
    {
        return await _context.RenduTests
            .Where(r => r.InscriptionId == inscriptionId)
            .Include(r => r.Test)
            .ThenInclude(t => t!.Module)
            .ThenInclude(m => m!.Formation)
            .Include(r => r.Inscription)
            .ThenInclude(i => i!.Edudiant)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RenduTest>> GetByTestIdAsync(int testId, CancellationToken cancellationToken)
    {
        return await _context.RenduTests
            .Where(r => r.TestId == testId)
            .Include(r => r.Test)
            .ThenInclude(t => t!.Module)
            .ThenInclude(m => m!.Formation)
            .Include(r => r.Inscription)
            .ThenInclude(i => i!.Edudiant)
            .ToListAsync(cancellationToken);
    }

    private IQueryable<RenduTest> QueryRendus()
        => _context.RenduTests
            .AsNoTracking()
            .Include(r => r.Test)
            .ThenInclude(t => t!.Module)
            .ThenInclude(m => m!.Formation)
            .Include(r => r.Inscription)
            .ThenInclude(i => i!.Edudiant);
}
