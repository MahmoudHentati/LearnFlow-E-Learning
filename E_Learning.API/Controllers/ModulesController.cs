using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController : ControllerBase
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IMapper _mapper;

    public ModulesController(IModuleRepository moduleRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(moduleRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _moduleRepository = moduleRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _moduleRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<ModuleDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _moduleRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<ModuleDTO>(entity));
    }

    [HttpGet("formation/{formationId:int}")]
    public async Task<IActionResult> GetByFormationIdAsync(int formationId, CancellationToken cancellationToken)
    {
        if (formationId <= 0) return BadRequest("Id de formation invalide.");

        var entities = await _moduleRepository.GetByFormationIdAsync(formationId, cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<ModuleDTO>>(entities));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ModuleDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Titre)) return BadRequest("Le titre est obligatoire.");

        var created = await _moduleRepository.CreateAsync(_mapper.Map<Module>(dto), cancellationToken);
        var createdDto = _mapper.Map<ModuleDTO>(created);
        return Created($"/api/modules/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] ModuleDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _moduleRepository.UpdateAsync(_mapper.Map<Module>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _moduleRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
