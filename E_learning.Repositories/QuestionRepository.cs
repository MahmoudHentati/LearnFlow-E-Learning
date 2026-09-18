using E_Learning.Domain.Entities;
using E_learning.Interfaces;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public QuestionRepository(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Question>> GetAllAsync(CancellationToken cancellationToken = default)
        => await QueryQuestions()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await QueryQuestions()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Question>> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken = default)
        => await QueryQuestions()
            .Where(x => x.QuizId == quizId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

    public async Task<Question> CreateAsync(Question question, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(question);
        await _dbContext.Questions.AddAsync(question, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return question;
    }

    public async Task<bool> UpdateAsync(Question question, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(question);
        var existing = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Id == question.Id, cancellationToken);
        if (existing is null) return false;

        existing.Enonce = question.Enonce;
        existing.Type = question.Type;
        existing.Bareme = question.Bareme;
        existing.QuizId = question.QuizId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Questions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (existing is null) return false;

        _dbContext.Questions.Remove(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private IQueryable<Question> QueryQuestions()
        => _dbContext.Questions
            .AsNoTracking()
            .Include(x => x.Quiz)
            .Include(x => x.OptionsReponses);
}
