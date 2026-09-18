using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizRepository _quizRepository;
    private readonly IMapper _mapper;

    public QuizzesController(IQuizRepository quizRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(quizRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _quizRepository = quizRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _quizRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<QuizDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _quizRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<QuizDTO>(entity));
    }

    [HttpGet("module/{moduleId:int}")]
    public async Task<IActionResult> GetByModuleIdAsync(int moduleId, CancellationToken cancellationToken)
    {
        if (moduleId <= 0) return BadRequest("Id de module invalide.");

        var entities = await _quizRepository.GetByModuleIdAsync(moduleId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<QuizDTO>>(entities));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] QuizDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var created = await _quizRepository.CreateAsync(_mapper.Map<Quiz>(dto), cancellationToken);
        var createdDto = _mapper.Map<QuizDTO>(created);
        return Created($"/api/quizzes/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] QuizDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _quizRepository.UpdateAsync(_mapper.Map<Quiz>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _quizRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
