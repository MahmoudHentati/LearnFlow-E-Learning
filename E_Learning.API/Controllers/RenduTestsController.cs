using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RenduTestsController : ControllerBase
{
    private readonly IRenduTestRepository _renduTestRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public RenduTestsController(IRenduTestRepository renduTestRepository, IMapper mapper, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(renduTestRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(environment);
        _renduTestRepository = renduTestRepository;
        _mapper = mapper;
        _environment = environment;
    }

    /// <summary>
    /// Retourne tous les rendus de tests.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var rendus = await _renduTestRepository.GetAllAsync(cancellationToken);
        var renduDtos = _mapper.Map<IReadOnlyList<RenduTestDTO>>(rendus);
        return Ok(renduDtos);
    }

    /// <summary>
    /// Retourne un rendu de test par identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var rendu = await _renduTestRepository.GetByIdAsync(id, cancellationToken);
        if (rendu is null)
        {
            return NotFound();
        }

        var renduDto = _mapper.Map<RenduTestDTO>(rendu);
        return Ok(renduDto);
    }

    /// <summary>
    /// Retourne les rendus liés au formateur connecté.
    /// </summary>
    [Authorize]
    [HttpGet("instructor/me")]
    public async Task<IActionResult> GetMyInstructorRendusAsync(CancellationToken cancellationToken)
    {
        var instructorId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(instructorId))
        {
            return Unauthorized();
        }

        var rendus = await _renduTestRepository.GetByInstructorIdAsync(instructorId, cancellationToken);
        var renduDtos = _mapper.Map<IReadOnlyList<RenduTestDTO>>(rendus);
        return Ok(renduDtos);
    }

    /// <summary>
    /// Cr�e un rendu de test.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] RenduTestDTO renduDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(renduDto);

        var rendu = _mapper.Map<RenduTest>(renduDto);
        var createdRendu = await _renduTestRepository.CreateAsync(rendu, cancellationToken);
        var createdRenduDto = _mapper.Map<RenduTestDTO>(createdRendu);
        return Created($"/api/rendutests/{createdRenduDto.Id}", createdRenduDto);
    }

    /// <summary>
    /// Crée ou met à jour un rendu étudiant avec import de fichier.
    /// </summary>
    [Authorize]
    [HttpPost("upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 25 * 1024 * 1024)]
    [RequestSizeLimit(25 * 1024 * 1024)]
    public async Task<IActionResult> SubmitWithUploadAsync([FromForm] SubmitRenduUploadRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.TestId <= 0)
        {
            return BadRequest("Le test est obligatoire.");
        }

        if (request.InscriptionId <= 0)
        {
            return BadRequest("L'inscription est obligatoire.");
        }

        if (string.IsNullOrWhiteSpace(request.Contenu) && request.Fichier is null)
        {
            return BadRequest("Ajoutez un message ou un fichier pour soumettre le rendu.");
        }

        var existingRendu = (await _renduTestRepository.GetByInscriptionIdAsync(request.InscriptionId, cancellationToken))
            .FirstOrDefault(r => r.TestId == request.TestId);

        var uploadedFileUrl = existingRendu?.FichierUrl ?? string.Empty;
        if (request.Fichier is not null && request.Fichier.Length > 0)
        {
            uploadedFileUrl = await SaveRenduFileAsync(request.Fichier, cancellationToken);
        }

        if (existingRendu is null)
        {
            var createdRendu = await _renduTestRepository.CreateAsync(new RenduTest
            {
                DateSoumission = DateTime.UtcNow,
                Contenu = request.Contenu?.Trim() ?? string.Empty,
                FichierUrl = uploadedFileUrl,
                TestId = request.TestId,
                InscriptionId = request.InscriptionId
            }, cancellationToken);

            var createdDto = _mapper.Map<RenduTestDTO>(createdRendu);
            return Created($"/api/rendutests/{createdDto.Id}", createdDto);
        }

        existingRendu.DateSoumission = DateTime.UtcNow;
        existingRendu.Contenu = request.Contenu?.Trim() ?? string.Empty;
        existingRendu.FichierUrl = uploadedFileUrl;

        var updated = await _renduTestRepository.UpdateAsync(existingRendu, cancellationToken);
        if (!updated)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Impossible de mettre à jour le rendu.");
        }

        var updatedDto = _mapper.Map<RenduTestDTO>(existingRendu);
        return Ok(updatedDto);
    }

    /// <summary>
    /// Met � jour un rendu de test.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] RenduTestDTO renduDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(renduDto);

        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        if (id != renduDto.Id)
        {
            return BadRequest("L'id de route et l'id du payload doivent correspondre.");
        }

        var rendu = _mapper.Map<RenduTest>(renduDto);
        var updated = await _renduTestRepository.UpdateAsync(rendu, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Supprime un rendu de test.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var deleted = await _renduTestRepository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Retourne les rendus pour une inscription.
    /// </summary>
    [HttpGet("inscription/{inscriptionId:int}")]
    public async Task<IActionResult> GetByInscriptionIdAsync(int inscriptionId, CancellationToken cancellationToken)
    {
        if (inscriptionId <= 0)
        {
            return BadRequest("Id d'inscription invalide.");
        }

        var rendus = await _renduTestRepository.GetByInscriptionIdAsync(inscriptionId, cancellationToken);
        var renduDtos = _mapper.Map<IReadOnlyList<RenduTestDTO>>(rendus);
        return Ok(renduDtos);
    }

    /// <summary>
    /// Retourne les rendus pour un test.
    /// </summary>
    [HttpGet("test/{testId:int}")]
    public async Task<IActionResult> GetByTestIdAsync(int testId, CancellationToken cancellationToken)
    {
        if (testId <= 0)
        {
            return BadRequest("Id de test invalide.");
        }

        var rendus = await _renduTestRepository.GetByTestIdAsync(testId, cancellationToken);
        var renduDtos = _mapper.Map<IReadOnlyList<RenduTestDTO>>(rendus);
        return Ok(renduDtos);
    }

    private string? GetCurrentUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? User.FindFirstValue("sub");

    private async Task<string> SaveRenduFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var rendusDirectory = Path.Combine(webRootPath, "uploads", "rendus");
        Directory.CreateDirectory(rendusDirectory);

        var extension = Path.GetExtension(file.FileName);
        var safeExtension = string.IsNullOrWhiteSpace(extension) ? ".bin" : extension;
        var storedFileName = $"{Guid.NewGuid():N}{safeExtension}";
        var destinationPath = Path.Combine(rendusDirectory, storedFileName);

        await using var stream = new FileStream(destinationPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"{Request.Scheme}://{Request.Host}/uploads/rendus/{storedFileName}";
    }

    public sealed class SubmitRenduUploadRequest
    {
        public string? Contenu { get; set; }
        public int TestId { get; set; }
        public int InscriptionId { get; set; }
        public IFormFile? Fichier { get; set; }
    }
}
