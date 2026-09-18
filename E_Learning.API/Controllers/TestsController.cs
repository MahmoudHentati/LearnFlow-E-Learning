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
public class TestsController : ControllerBase
{
    private readonly ITestRepository _testRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public TestsController(ITestRepository testRepository, IMapper mapper, IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(testRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(environment);
        _testRepository = testRepository;
        _mapper = mapper;
        _environment = environment;
    }

    /// <summary>
    /// Retourne tous les tests.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var tests = await _testRepository.GetAllAsync(cancellationToken);
        var testDtos = _mapper.Map<IReadOnlyList<TestDTO>>(tests);
        return Ok(testDtos);
    }

    /// <summary>
    /// Retourne un test par identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var test = await _testRepository.GetByIdAsync(id, cancellationToken);
        if (test is null)
        {
            return NotFound();
        }

        var testDto = _mapper.Map<TestDTO>(test);
        return Ok(testDto);
    }

    /// <summary>
    /// Retourne les tests du formateur connecté.
    /// </summary>
    [Authorize]
    [HttpGet("instructor/me")]
    public async Task<IActionResult> GetMyTestsAsync(CancellationToken cancellationToken)
    {
        var instructorId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(instructorId))
        {
            return Unauthorized();
        }

        var tests = await _testRepository.GetByInstructorIdAsync(instructorId, cancellationToken);
        var testDtos = _mapper.Map<IReadOnlyList<TestDTO>>(tests);
        return Ok(testDtos);
    }

    /// <summary>
    /// Cr�e un test.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] TestDTO testDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(testDto);

        if (string.IsNullOrWhiteSpace(testDto.Consigne))
        {
            return BadRequest("Le titre est obligatoire.");
        }

        var test = _mapper.Map<Test>(testDto);
        var createdTest = await _testRepository.CreateAsync(test, cancellationToken);
        var createdTestDto = _mapper.Map<TestDTO>(createdTest);
        return Created($"/api/tests/{createdTestDto.Id}", createdTestDto);
    }

    /// <summary>
    /// Crée un test avec import de fichier stocké dans wwwroot/uploads/tests.
    /// </summary>
    [Authorize]
    [HttpPost("upload")]
    [RequestFormLimits(MultipartBodyLengthLimit = 25 * 1024 * 1024)]
    [RequestSizeLimit(25 * 1024 * 1024)]
    public async Task<IActionResult> CreateWithUploadAsync([FromForm] CreateTestUploadRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Consigne))
        {
            return BadRequest("La consigne est obligatoire.");
        }

        if (request.ModuleId <= 0)
        {
            return BadRequest("Le module est obligatoire.");
        }

        var fileUrl = string.Empty;
        if (request.Fichier is not null && request.Fichier.Length > 0)
        {
            fileUrl = await SaveTestFileAsync(request.Fichier, cancellationToken);
        }

        var test = new Test
        {
            Consigne = request.Consigne.Trim(),
            ModuleId = request.ModuleId,
            FichierConsigneUrl = fileUrl
        };

        var createdTest = await _testRepository.CreateAsync(test, cancellationToken);
        var createdTestDto = _mapper.Map<TestDTO>(createdTest);
        return Created($"/api/tests/{createdTestDto.Id}", createdTestDto);
    }

    /// <summary>
    /// Met � jour un test.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] TestDTO testDto, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(testDto);

        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        if (id != testDto.Id)
        {
            return BadRequest("L'id de route et l'id du payload doivent correspondre.");
        }

        var test = _mapper.Map<Test>(testDto);
        var updated = await _testRepository.UpdateAsync(test, cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Supprime un test.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return BadRequest("Id invalide.");
        }

        var existing = await _testRepository.GetByIdAsync(id, cancellationToken);
        var deleted = await _testRepository.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        DeletePhysicalFile(existing?.FichierConsigneUrl);

        return NoContent();
    }

    /// <summary>
    /// Retourne les tests d'un module.
    /// </summary>
    [HttpGet("module/{moduleId:int}")]
    public async Task<IActionResult> GetByModuleIdAsync(int moduleId, CancellationToken cancellationToken)
    {
        if (moduleId <= 0)
        {
            return BadRequest("Id de module invalide.");
        }

        var tests = await _testRepository.GetByModuleIdAsync(moduleId, cancellationToken);
        var testDtos = _mapper.Map<IReadOnlyList<TestDTO>>(tests);
        return Ok(testDtos);
    }

    private string? GetCurrentUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? User.FindFirstValue("sub");

    private async Task<string> SaveTestFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var testsDirectory = Path.Combine(webRootPath, "uploads", "tests");
        Directory.CreateDirectory(testsDirectory);

        var extension = Path.GetExtension(file.FileName);
        var safeExtension = string.IsNullOrWhiteSpace(extension) ? ".bin" : extension;
        var storedFileName = $"{Guid.NewGuid():N}{safeExtension}";
        var destinationPath = Path.Combine(testsDirectory, storedFileName);

        await using var stream = new FileStream(destinationPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"{Request.Scheme}://{Request.Host}/uploads/tests/{storedFileName}";
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

    public sealed class CreateTestUploadRequest
    {
        public string Consigne { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public IFormFile? Fichier { get; set; }
    }
}
