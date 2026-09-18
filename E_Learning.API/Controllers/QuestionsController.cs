using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public QuestionsController(IQuestionRepository questionRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(questionRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _questionRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<QuestionDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _questionRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<QuestionDTO>(entity));
    }

    [HttpGet("quiz/{quizId:int}")]
    public async Task<IActionResult> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken)
    {
        if (quizId <= 0) return BadRequest("Id de quiz invalide.");

        var entities = await _questionRepository.GetByQuizIdAsync(quizId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<QuestionDTO>>(entities));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] QuestionDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Enonce)) return BadRequest("L'énoncé est obligatoire.");

        var created = await _questionRepository.CreateAsync(_mapper.Map<Question>(dto), cancellationToken);
        var createdDto = _mapper.Map<QuestionDTO>(created);
        return Created($"/api/questions/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] QuestionDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _questionRepository.UpdateAsync(_mapper.Map<Question>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _questionRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
