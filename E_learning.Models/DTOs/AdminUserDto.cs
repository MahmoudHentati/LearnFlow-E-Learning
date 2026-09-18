namespace E_learning.Models.DTOs;

public sealed record AdminUserDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive
);
