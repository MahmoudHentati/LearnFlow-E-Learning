namespace E_learning.Models.DTOs;

public sealed record UpdateUserRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Role,
    bool IsActive
);
