using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models;
using E_learning.Models.DTOs;
using E_learning.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormationController : ControllerBase
{
    private readonly IFormationRepository _formationRepository;
    private readonly ICategorieRepository _categorieRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public FormationController(
        IFormationRepository formationRepository,
        ICategorieRepository categorieRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(formationRepository);
        ArgumentNullException.ThrowIfNull(categorieRepository);
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _formationRepository = formationRepository;
        _categorieRepository = categorieRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retourne toutes les formations.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var formations = await _formationRepository.GetAllAsync(cancellationToken);
        var formationDtos = _mapper.Map<IReadOnlyList<FormationDTO>>(formations);
        return Ok(formationDtos);
    }

    /// <summary>
    /// Retourne une formation par identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var formation = await _formationRepository.GetByIdAsync(id, cancellationToken);
        if (formation is null)
        {
            return NotFound();
        }

        var formationDto = _mapper.Map<FormationDTO>(formation);
        return Ok(formationDto);
    }

    /// <summary>
    /// Retourne les statistiques publiques de la page d'accueil.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("home-summary")]
    public async Task<IActionResult> GetHomeSummaryAsync(CancellationToken cancellationToken)
    {
        var formations = await _formationRepository.GetAllAsync(cancellationToken);
        var categories = await _categorieRepository.GetAllAsync(cancellationToken);
        var students = await _userRepository.GetUsersByRoleAsync(AppRoles.Student, cancellationToken);

        var publishedCount = formations.Count(formation => FormationStatusHelper.IsActive(formation.Statut));

        return Ok(new HomeSummaryDto
        {
            PublishedFormationsCount = publishedCount,
            StudentCount = students.Count,
            CategoryCount = categories.Count
        });
    }

    /// <summary>
    /// Crée une formation.
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] FormationDTO formationDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(formationDto);

        if (string.IsNullOrWhiteSpace(formationDto.Titre))
        {
            return BadRequest("Le titre est obligatoire.");
        }

        var formation = _mapper.Map<Formation>(formationDto);
        formation.Statut = FormationStatusHelper.NormalizeOrDefault(formation.Statut);
        var currentUserId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized();
        }

        formation.FormateurId = ResolveFormateurId(formationDto.FormateurId, currentUserId);

        if (string.IsNullOrWhiteSpace(formation.FormateurId))
        {
            return BadRequest("Le formateur est obligatoire.");
        }

        var createdFormation = await _formationRepository.CreateAsync(formation, cancellationToken);
        var createdFormationDto = _mapper.Map<FormationDTO>(createdFormation);
        return Created($"/api/formation/{createdFormationDto.Id}", createdFormationDto);
    }

    /// <summary>
    /// Met à jour une formation.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] FormationDTO formationDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(formationDto);

        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        if (id != formationDto.Id)
        {
            return BadRequest("L'id de route et l'id du payload doivent correspondre.");
        }

        var formation = _mapper.Map<Formation>(formationDto);
        formation.Statut = FormationStatusHelper.NormalizeOrDefault(formation.Statut);
        var currentUserId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized();
        }

        formation.FormateurId = ResolveFormateurId(formationDto.FormateurId, currentUserId);
        if (string.IsNullOrWhiteSpace(formation.FormateurId))
        {
            return BadRequest("Le formateur est obligatoire.");
        }

        var updated = await _formationRepository.UpdateAsync(formation, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Supprime une formation.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var deleted = await _formationRepository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Retourne les formations de l'utilisateur connecté.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyFormationsAsync(CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized();
        }

        var formations = await _formationRepository.GetByFormateurIdAsync(currentUserId, cancellationToken);
        var formationDtos = _mapper.Map<IReadOnlyList<FormationDTO>>(formations);
        return Ok(formationDtos);
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? User.FindFirstValue("sub");
    }

    private string ResolveFormateurId(string? requestedFormateurId, string currentUserId)
    {
        if ((User.IsInRole(AppRoles.Admin) || User.IsInRole(AppRoles.SuperAdmin)) &&
            !string.IsNullOrWhiteSpace(requestedFormateurId))
        {
            return requestedFormateurId;
        }

        return currentUserId;
    }

    public sealed class HomeSummaryDto
    {
        public int PublishedFormationsCount { get; set; }
        public int StudentCount { get; set; }
        public int CategoryCount { get; set; }
    }
}
