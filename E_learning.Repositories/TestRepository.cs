using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class TestRepository : ITestRepository
{
    private readonly ApplicationDbContext _context;

    public TestRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task<IReadOnlyList<Test>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Tests
            .AsNoTracking()
            .Include(t => t.Module)
            .ThenInclude(m => m!.Formation)
            .ToListAsync(cancellationToken);
    }

    public async Task<Test?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Tests
            .AsNoTracking()
            .Include(t => t.Module)
            .ThenInclude(m => m!.Formation)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Test>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(instructorId))
        {
            return [];
        }

        return await _context.Tests
            .AsNoTracking()
            .Include(t => t.Module)
            .ThenInclude(m => m!.Formation)
            .Where(t => t.Module != null &&
                        t.Module.Formation != null &&
                        t.Module.Formation.FormateurId == instructorId)
            .OrderBy(t => t.ModuleId)
            .ThenBy(t => t.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Test> CreateAsync(Test test, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(test);

        _context.Tests.Add(test);
        await _context.SaveChangesAsync(cancellationToken);
        return test;
    }

    public async Task<bool> UpdateAsync(Test test, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(test);

        var existing = await _context.Tests.FirstOrDefaultAsync(t => t.Id == test.Id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.Consigne = test.Consigne;
        existing.FichierConsigneUrl = test.FichierConsigneUrl;
        existing.ModuleId = test.ModuleId;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var test = await _context.Tests.FindAsync(new object[] { id }, cancellationToken);
        if (test is null)
        {
            return false;
        }

        _context.Tests.Remove(test);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IReadOnlyList<Test>> GetByModuleIdAsync(int moduleId, CancellationToken cancellationToken)
    {
        return await _context.Tests
            .AsNoTracking()
            .Where(t => t.ModuleId == moduleId)
            .Include(t => t.Module)
            .ThenInclude(m => m!.Formation)
            .ToListAsync(cancellationToken);
    }
}
