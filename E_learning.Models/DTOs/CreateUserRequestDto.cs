namespace E_learning.Models.DTOs;

public sealed record CreateUserRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Role
);
