using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SUPERADMIN,Instructor")]
public class SousCategoriesController : ControllerBase
{
    private readonly ISousCategorieRepository _sousCategorieRepository;
    private readonly IMapper _mapper;

    public SousCategoriesController(ISousCategorieRepository sousCategorieRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(sousCategorieRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _sousCategorieRepository = sousCategorieRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _sousCategorieRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<SousCategorieDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _sousCategorieRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<SousCategorieDTO>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] SousCategorieDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Nom)) return BadRequest("Le nom est obligatoire.");

        var created = await _sousCategorieRepository.CreateAsync(_mapper.Map<SousCategorie>(dto), cancellationToken);
        var createdDto = _mapper.Map<SousCategorieDTO>(created);
        return Created($"/api/souscategories/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] SousCategorieDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _sousCategorieRepository.UpdateAsync(_mapper.Map<SousCategorie>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _sousCategorieRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
