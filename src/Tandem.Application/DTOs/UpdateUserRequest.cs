namespace Tandem.Application.DTOs;

public record UpdateUserRequest(
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNumber);

