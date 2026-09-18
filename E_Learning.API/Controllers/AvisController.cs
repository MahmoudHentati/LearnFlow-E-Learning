using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvisController : ControllerBase
{
    private readonly IAvisRepository _avisRepository;
    private readonly IMapper _mapper;

    public AvisController(IAvisRepository avisRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(avisRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _avisRepository = avisRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _avisRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<AvisDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _avisRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<AvisDTO>(entity));
    }

    [HttpGet("inscription/{inscriptionId:int}")]
    public async Task<IActionResult> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken)
    {
        if (inscriptionId <= 0) return BadRequest("Id d'inscription invalide.");

        var entities = await _avisRepository.GetByInscriptionIdAsync(inscriptionId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<AvisDTO>>(entities));
    }

    [HttpGet("formation/{formationId:int}")]
    public async Task<IActionResult> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken)
    {
        if (formationId <= 0) return BadRequest("Id de formation invalide.");

        var entities = await _avisRepository.GetByFormationIdAsync(formationId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<AvisDTO>>(entities));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AvisDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var created = await _avisRepository.CreateAsync(_mapper.Map<Avis>(dto), cancellationToken);
        var createdDto = _mapper.Map<AvisDTO>(created);
        return Created($"/api/avis/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] AvisDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _avisRepository.UpdateAsync(_mapper.Map<Avis>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _avisRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
