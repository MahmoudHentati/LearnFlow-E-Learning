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
public class VideosController : ControllerBase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public VideosController(IVideoRepository videoRepository, IMapper mapper, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(videoRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(environment);
        _videoRepository = videoRepository;
        _mapper = mapper;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var entities = await _videoRepository.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IReadOnlyList<VideoDTO>>(entities));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var entity = await _videoRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return NotFound();

        return Ok(_mapper.Map<VideoDTO>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] VideoDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (string.IsNullOrWhiteSpace(dto.Titre)) return BadRequest("Le titre est obligatoire.");

        var created = await _videoRepository.CreateAsync(_mapper.Map<Video>(dto), cancellationToken);
        var createdDto = _mapper.Map<VideoDTO>(created);
        return Created($"/api/videos/{createdDto.Id}", createdDto);
    }

    [Authorize]
    [HttpPost("upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 100 * 1024 * 1024)]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> CreateWithUploadAsync([FromForm] CreateVideoUploadRequest request, CancellationToken cancellationToken)
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
            return BadRequest("Le fichier vidéo est obligatoire.");
        }

        var fileUrl = await SaveVideoFileAsync(request.Fichier, cancellationToken);

        var video = new Video
        {
            Titre = request.Titre.Trim(),
            Ordre = request.Ordre <= 0 ? 1 : request.Ordre,
            Duree = Math.Max(0, request.Duree),
            ModuleId = request.ModuleId,
            UrlVideo = fileUrl
        };

        var created = await _videoRepository.CreateAsync(video, cancellationToken);
        var createdDto = _mapper.Map<VideoDTO>(created);
        return Created($"/api/videos/{createdDto.Id}", createdDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] VideoDTO dto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0) return BadRequest("Id invalide.");
        if (id != dto.Id) return BadRequest("L'id de route et l'id du payload doivent correspondre.");

        var updated = await _videoRepository.UpdateAsync(_mapper.Map<Video>(dto), cancellationToken);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest("Id invalide.");

        var existing = await _videoRepository.GetByIdAsync(id, cancellationToken);
        var deleted = await _videoRepository.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();

        DeletePhysicalFile(existing?.UrlVideo);

        return NoContent();
    }

    private async Task<string> SaveVideoFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var videosDirectory = Path.Combine(webRootPath, "uploads", "videos");
        Directory.CreateDirectory(videosDirectory);

        var extension = Path.GetExtension(file.FileName);
        var safeExtension = string.IsNullOrWhiteSpace(extension) ? ".bin" : extension;
        var storedFileName = $"{Guid.NewGuid():N}{safeExtension}";
        var destinationPath = Path.Combine(videosDirectory, storedFileName);

        await using var stream = new FileStream(destinationPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"{Request.Scheme}://{Request.Host}/uploads/videos/{storedFileName}";
    }

    private void DeletePhysicalFile(string? publicUrl)
    {
        if (string.IsNullOrWhiteSpace(publicUrl) || !Uri.TryCreate(publicUrl, UriKind.Absolute, out var uri))
        {
            return;
        }

        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var relativePath = uri.AbsolutePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(webRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    public sealed class CreateVideoUploadRequest
    {
        public string Titre { get; set; } = string.Empty;
        public int Ordre { get; set; }
        public int Duree { get; set; }
        public int ModuleId { get; set; }
        public IFormFile? Fichier { get; set; }
    }
}
