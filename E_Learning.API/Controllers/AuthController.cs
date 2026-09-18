using E_Learning.Domain;
using E_learning.Models;
using E_learning.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AuthController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("roles")]
    public async Task<ActionResult<IEnumerable<string>>> GetCurrentUserRoles()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult> GetUserProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.IsActive,
            Roles = roles
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = ResolveRole(request.Role) is not AppRoles.Instructor
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }

        // Add role
        var canonicalRole = ResolveRole(request.Role);
        if (canonicalRole is not null)
        {
            await _userManager.AddToRoleAsync(user, canonicalRole);
        }

        return Ok(new
        {
            Message = canonicalRole == AppRoles.Instructor
                ? "Compte formateur créé. Validation administrateur en attente."
                : "User registered successfully"
        });
    }

    private static string? ResolveRole(string? role) => role?.ToLowerInvariant() switch
    {
        "student" or "etudiant" => AppRoles.Student,
        "instructor" or "formateur" => AppRoles.Instructor,
        "admin" => AppRoles.Admin,
        "superadmin" => AppRoles.SuperAdmin,
        _ => null
    };

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(string.Join("\n", errors));
        }

        return Ok(new { Message = "Mot de passe mis à jour avec succès" });
    }
}

public class ChangePasswordRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;
}
