using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class ParticipationQuizRepository : IParticipationQuizRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipationQuizRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task<IReadOnlyList<ParticipationQuiz>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.ParticipationQuizzes
            .Include(p => p.Quiz)
            .Include(p => p.Inscription)
            .ToListAsync(cancellationToken);
    }

    public async Task<ParticipationQuiz?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.ParticipationQuizzes
            .Include(p => p.Quiz)
            .Include(p => p.Inscription)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ParticipationQuiz> CreateAsync(ParticipationQuiz participationQuiz, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(participationQuiz);

        _context.ParticipationQuizzes.Add(participationQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return participationQuiz;
    }

    public async Task<bool> UpdateAsync(ParticipationQuiz participationQuiz, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(participationQuiz);

        _context.ParticipationQuizzes.Update(participationQuiz);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var participationQuiz = await _context.ParticipationQuizzes.FindAsync(new object[] { id }, cancellationToken);
        if (participationQuiz is null)
        {
            return false;
        }

        _context.ParticipationQuizzes.Remove(participationQuiz);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IReadOnlyList<ParticipationQuiz>> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken)
    {
        return await _context.ParticipationQuizzes
            .Where(p => p.InscriptionId == inscriptionId)
            .Include(p => p.Quiz)
            .Include(p => p.Inscription)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ParticipationQuiz>> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken)
    {
        return await _context.ParticipationQuizzes
            .Where(p => p.QuizId == quizId)
            .Include(p => p.Quiz)
            .Include(p => p.Inscription)
            .ToListAsync(cancellationToken);
    }
}