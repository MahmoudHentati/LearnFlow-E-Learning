using AutoMapper;
using E_learning.Interfaces;
using E_learning.Models;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionsController : ControllerBase
{
    private readonly IInscriptionRepository _inscriptionRepository;
    private readonly IMapper _mapper;

    public InscriptionsController(IInscriptionRepository inscriptionRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(inscriptionRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _inscriptionRepository = inscriptionRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _inscriptionRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<InscriptionDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _inscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<InscriptionDTO>(entity));
    }

    [Authorize(Roles = AppRoles.Student + "," + AppRoles.Admin + "," + AppRoles.SuperAdmin)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] InscriptionDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var created = await _inscriptionRepository.CreateAsync(_mapper.Map<Inscription>(dto), cancellationToken);
        var createdDto = _mapper.Map<InscriptionDTO>(created);
        return Created($"/api/inscriptions/{createdDto.Id}", createdDto);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyInscriptionsAsync(CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized();
        }

        var inscriptions = await _inscriptionRepository.GetByStudentIdAsync(currentUserId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<InscriptionDTO>>(inscriptions));
    }

    [Authorize(Roles = AppRoles.Student)]
    [HttpPost("me/{formationId:int}")]
    public async Task<IActionResult> EnrollCurrentUserAsync(int formationId, CancellationToken cancellationToken)
    {
        if (formationId <= 0) return BadRequest("Id de formation invalide.");

        var currentUserId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized();
        }

        var existing = await _inscriptionRepository.GetByStudentAndFormationAsync(currentUserId, formationId, cancellationToken);
        if (existing is not null)
        {
            return Ok(_mapper.Map<InscriptionDTO>(existing));
        }

        var created = await _inscriptionRepository.CreateAsync(new Inscription
        {
            DateInscription = DateTime.UtcNow,
            Progression = 0,
            EdudiantId = currentUserId,
            FormationId = formationId
        }, cancellationToken);

        var createdDto = _mapper.Map<InscriptionDTO>(created);
        return Created($"/api/inscriptions/{createdDto.Id}", createdDto);
    }

    [Authorize]
    [HttpGet("formation/{formationId:int}")]
    public async Task<IActionResult> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken)
    {
        if (formationId <= 0) return BadRequest("Id de formation invalide.");

        var inscriptions = await _inscriptionRepository.GetByFormationIdAsync(formationId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<InscriptionDTO>>(inscriptions));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] InscriptionDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _inscriptionRepository.UpdateAsync(_mapper.Map<Inscription>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _inscriptionRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }

    private string? GetCurrentUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? User.FindFirstValue("sub");
}
