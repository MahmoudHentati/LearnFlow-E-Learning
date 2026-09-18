using E_Learning.Domain;
using E_learning.Interfaces;
using E_learning.Models;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retourne tous les utilisateurs.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync(cancellationToken);
        var dtos = new List<AdminUserDto>(users.Count);

        foreach (var user in users)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roles = await _userRepository.GetRolesAsync(user.Id, cancellationToken);
            var role = roles.FirstOrDefault() ?? string.Empty;
            dtos.Add(ToAdminUserDto(user, role));
        }

        dtos = dtos
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToList();

        return Ok(dtos);
    }

    /// <summary>
    /// Retourne la liste des utilisateurs d'un rôle donné.
    /// </summary>
    [HttpGet("by-role/{role}")]
    public async Task<IActionResult> GetByRoleAsync(string role, CancellationToken cancellationToken)
    {
        var resolvedRole = ResolveRole(role);
        if (resolvedRole is null)
        {
            return BadRequest("Rôle invalide.");
        }

        var users = await _userRepository.GetUsersByRoleAsync(resolvedRole, cancellationToken);
        var dtos = users
            .Select(user => ToAdminUserDto(user, resolvedRole))
            .OrderBy(x => x.LastName)
            .ThenBy(x => x.FirstName)
            .ToList();

        return Ok(dtos);
    }

    /// <summary>
    /// Retourne un utilisateur par identifiant.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id invalide.");
        }

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        var roles = await _userRepository.GetRolesAsync(id, cancellationToken);
        var role = roles.FirstOrDefault() ?? string.Empty;

        return Ok(ToAdminUserDto(user, role));
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email et mot de passe sont obligatoires.");
        }

        var resolvedRole = ResolveRole(request.Role);
        if (resolvedRole is null)
        {
            return BadRequest("Rôle invalide.");
        }

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true
        };

        var result = await _userRepository.CreateAsync(user, request.Password, resolvedRole, cancellationToken);
        if (!result.Success || result.User is null)
        {
            return BadRequest(new { Errors = result.Errors });
        }

        var dto = ToAdminUserDto(result.User, resolvedRole);
        return Created($"/api/users/{dto.Id}", dto);
    }

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id invalide.");
        }

        var resolvedRole = ResolveRole(request.Role);
        if (resolvedRole is null)
        {
            return BadRequest("Rôle invalide.");
        }

        var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (existingUser is null)
        {
            return NotFound();
        }

        existingUser.FirstName = request.FirstName;
        existingUser.LastName = request.LastName;
        existingUser.Email = request.Email;
        existingUser.UserName = request.Email;
        existingUser.IsActive = request.IsActive;

        var result = await _userRepository.UpdateAsync(existingUser, resolvedRole, cancellationToken);
        if (!result.Success)
        {
            return BadRequest(new { Errors = result.Errors });
        }

        return NoContent();
    }

    /// <summary>
    /// Supprime un utilisateur.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Id invalide.");
        }

        var result = await _userRepository.DeleteAsync(id, cancellationToken);
        if (!result.Success)
        {
            return NotFound(new { Errors = result.Errors });
        }

        return NoContent();
    }

    private static string? ResolveRole(string? role) => role?.ToLowerInvariant() switch
    {
        "student" or "etudiant" => AppRoles.Student,
        "instructor" or "formateur" => AppRoles.Instructor,
        "admin" => AppRoles.Admin,
        "superadmin" => AppRoles.SuperAdmin,
        _ => null
    };

    private static AdminUserDto ToAdminUserDto(AppUser user, string role)
        => new(
            user.Id,
            user.FirstName ?? string.Empty,
            user.LastName ?? string.Empty,
            user.Email ?? string.Empty,
            role,
            user.IsActive
        );
}
