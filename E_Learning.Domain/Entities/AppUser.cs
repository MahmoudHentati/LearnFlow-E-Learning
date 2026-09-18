using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Domain;

public class AppUser : IdentityUser
{
    [MaxLength(100)]
    public string? FirstName { get; set; }
    
    [MaxLength(100)]
    public string? LastName { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; } = true;
    public long? TagNumber { get; set; }
   
    
    // Soft delete support for users
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    
    [MaxLength(450)]
    public string? DeletedBy { get; set; }
    
}
