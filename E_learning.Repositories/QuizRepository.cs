using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly ApplicationDbContext _dbContext;

    public QuizRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Quiz>> GetAllAsync(CancellationToken cancellationToken = default)
        => await QueryQuizzes()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Quiz?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await QueryQuizzes()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Quiz>> GetByModuleIdAsync(int moduleId, CancellationToken cancellationToken = default)
        => await QueryQuizzes()
            .Where(x => x.ModuleId == moduleId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Quiz> CreateAsync(Quiz quiz, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(quiz);
        await _dbContext.Quizzes.AddAsync(quiz, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return quiz;
    }

    public async Task<bool> UpdateAsync(Quiz quiz, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(quiz);
        var existing = await _dbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == quiz.Id, cancellationToken);
        if (existing is null) return false;

        existing.DLimite = quiz.DLimite;
        existing.ScoreMinimum = quiz.ScoreMinimum;
        existing.ModuleId = quiz.ModuleId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Quizzes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Quizzes.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private IQueryable<Quiz> QueryQuizzes()
        => _dbContext.Quizzes
            .AsNoTracking()
            .Include(x => x.Module)
            .Include(x => x.Questions)
            .ThenInclude(question => question.OptionsReponses);
}
