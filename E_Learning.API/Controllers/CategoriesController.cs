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
public class CategoriesController : ControllerBase
{
    private readonly ICategorieRepository _categorieRepository;
    private readonly IMapper _mapper;

    public CategoriesController(ICategorieRepository categorieRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(categorieRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _categorieRepository = categorieRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _categorieRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<CategorieDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _categorieRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<CategorieDTO>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CategorieDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Nom)) return BadRequest("Le nom est obligatoire.");

        var created = await _categorieRepository.CreateAsync(_mapper.Map<Categorie>(dto), cancellationToken);
        var createdDto = _mapper.Map<CategorieDTO>(created);
        return Created($"/api/categories/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategorieDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _categorieRepository.UpdateAsync(_mapper.Map<Categorie>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var deleted = await _categorieRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
