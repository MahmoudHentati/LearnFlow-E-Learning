using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Persistence;
using Microsoft.EntityFrameworkCore;

namespace E_learning.Repositories;

public class OptionReponseRepository : IOptionReponseRepository
{
    private readonly ApplicationDbContext _context;

    public OptionReponseRepository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task<IReadOnlyList<OptionReponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Reponses
            .Include(o => o.Question)
            .ToListAsync(cancellationToken);
    }

    public async Task<OptionReponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Reponses
            .Include(o => o.Question)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<OptionReponse> CreateAsync(OptionReponse optionReponse, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(optionReponse);

        _context.Reponses.Add(optionReponse);
        await _context.SaveChangesAsync(cancellationToken);
        return optionReponse;
    }

    public async Task<bool> UpdateAsync(OptionReponse optionReponse, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(optionReponse);

        _context.Reponses.Update(optionReponse);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var optionReponse = await _context.Reponses.FindAsync(new object[] { id }, cancellationToken);
        if (optionReponse is null)
        {
            return false;
        }

        _context.Reponses.Remove(optionReponse);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<IReadOnlyList<OptionReponse>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken)
    {
        return await _context.Reponses
            .Where(o => o.QuestionId == questionId)
            .Include(o => o.Question)
            .ToListAsync(cancellationToken);
    }
}