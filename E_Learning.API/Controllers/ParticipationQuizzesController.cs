using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipationQuizzesController : ControllerBase
{
    private readonly IParticipationQuizRepository _participationQuizRepository;
    private readonly IMapper _mapper;

    public ParticipationQuizzesController(IParticipationQuizRepository participationQuizRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(participationQuizRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _participationQuizRepository = participationQuizRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retourne toutes les participations � des quiz.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var participations = await _participationQuizRepository.GetAllAsync(cancellationToken);
        var participationDtos = _mapper.Map<IReadOnlyList<ParticipationQuizDTO>>(participations);
        return Ok(participationDtos);
    }

    /// <summary>
    /// Retourne une participation par identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var participation = await _participationQuizRepository.GetByIdAsync(id, cancellationToken);
        if (participation is null)
        {
            return NotFound();
        }

        var participationDto = _mapper.Map<ParticipationQuizDTO>(participation);
        return Ok(participationDto);
    }

    /// <summary>
    /// Cr�e une participation � un quiz.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ParticipationQuizDTO participationDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(participationDto);

        var participation = _mapper.Map<ParticipationQuiz>(participationDto);
        var createdParticipation = await _participationQuizRepository.CreateAsync(participation, cancellationToken);
        var createdParticipationDto = _mapper.Map<ParticipationQuizDTO>(createdParticipation);
        return Created($"/api/participationquizzes/{createdParticipationDto.Id}", createdParticipationDto);
    }

    /// <summary>
    /// Met � jour une participation.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ParticipationQuizDTO participationDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(participationDto);

        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        if (id != participationDto.Id)
        {
            return BadRequest("L'id de route et l'id du payload doivent correspondre.");
        }

        var participation = _mapper.Map<ParticipationQuiz>(participationDto);
        var updated = await _participationQuizRepository.UpdateAsync(participation, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Supprime une participation.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var deleted = await _participationQuizRepository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Retourne les participations d'une inscription.
    /// </summary>
    [HttpGet("inscription/{inscriptionId:int}")]
    public async Task<IActionResult> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken)
    {
        if (inscriptionId <= 0)
        {
            return BadRequest("Id d'inscription invalide.");
        }

        var participations = await _participationQuizRepository.GetByInscriptionIdAsync(inscriptionId, cancellationToken);
        var participationDtos = _mapper.Map<IReadOnlyList<ParticipationQuizDTO>>(participations);
        return Ok(participationDtos);
    }

    /// <summary>
    /// Retourne les participations pour un quiz.
    /// </summary>
    [HttpGet("quiz/{quizId:int}")]
    public async Task<IActionResult> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken)
    {
        if (quizId <= 0)
        {
            return BadRequest("Id de quiz invalide.");
        }

        var participations = await _participationQuizRepository.GetByQuizIdAsync(quizId, cancellationToken);
        var participationDtos = _mapper.Map<IReadOnlyList<ParticipationQuizDTO>>(participations);
        return Ok(participationDtos);
    }
}
