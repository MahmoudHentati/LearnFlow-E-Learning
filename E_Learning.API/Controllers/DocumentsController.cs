using AutoMapper;
using E_learning.Interfaces;
using E_Learning.Domain.Entities;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public DocumentsController(IDocumentRepository documentRepository, IMapper mapper, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(documentRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(environment);
        _documentRepository = documentRepository;
        _mapper = mapper;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _documentRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<DocumentDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _documentRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<DocumentDTO>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] DocumentDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Titre)) return BadRequest("Le titre est obligatoire.");

        var created = await _documentRepository.CreateAsync(_mapper.Map<Document>(dto), cancellationToken);
        var createdDto = _mapper.Map<DocumentDTO>(created);
        return Created($"/api/documents/{createdDto.Id}", createdDto);
    }

    [Authorize]
    [HttpPost("upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 25 * 1024 * 1024)]
    [RequestSizeLimit(25 * 1024 * 1024)]
    public async Task<IActionResult> CreateWithUploadAsync([FromForm] CreateDocumentUploadRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Titre))
        {
            return BadRequest("Le titre est obligatoire.");
        }

        if (request.ModuleId <= 0)
        {
            return BadRequest("Le module est obligatoire.");
        }

        if (request.Fichier is null || request.Fichier.Length == 0)
        {
            return BadRequest("Le fichier est obligatoire.");
        }

        var fileUrl = await SaveDocumentFileAsync(request.Fichier, cancellationToken);
        var format = Path.GetExtension(request.Fichier.FileName).TrimStart('.');

        var document = new Document
        {
            Titre = request.Titre.Trim(),
            Ordre = request.Ordre <= 0 ? 1 : request.Ordre,
            ModuleId = request.ModuleId,
            UrlFichier = fileUrl,
            Format = string.IsNullOrWhiteSpace(format) ? "fichier" : format
        };

        var created = await _documentRepository.CreateAsync(document, cancellationToken);
        var createdDto = _mapper.Map<DocumentDTO>(created);
        return Created($"/api/documents/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] DocumentDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _documentRepository.UpdateAsync(_mapper.Map<Document>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var existing = await _documentRepository.GetByIdAsync(id, cancellationToken);
        var deleted = await _documentRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        DeletePhysicalFile(existing?.UrlFichier);

        return NoContent();
    }

    private async Task<string> SaveDocumentFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var documentsDirectory = Path.Combine(webRootPath, "uploads", "documents");
        Directory.CreateDirectory(documentsDirectory);

        var extension = Path.GetExtension(file.FileName);
        var safeExtension = string.IsNullOrWhiteSpace(extension) ? ".bin" : extension;
        var storedFileName = $"{Guid.NewGuid():N}{safeExtension}";
        var destinationPath = Path.Combine(documentsDirectory, storedFileName);

        await using var stream = new FileStream(destinationPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"{Request.Scheme}://{Request.Host}/uploads/documents/{storedFileName}";
    }

    private void DeletePhysicalFile(string? publicUrl)
    {
        if (string.IsNullOrWhiteSpace(publicUrl) || !Uri.TryCreate(publicUrl, UriKind.Absolute, out var uri))
        {
            return;
        }

        var relativePath = uri.AbsolutePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var fullPath = Path.Combine(webRootPath, relativePath);

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    public sealed class CreateDocumentUploadRequest
    {
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public int ModuleId { get; set; }
        public IFormFile? Fichier { get; set; }
    }
}
