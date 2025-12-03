namespace Tandem.Application.DTOs;

public record CreateUserRequest(
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNumber,
    string EmailAddress);

