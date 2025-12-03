namespace Tandem.Application.DTOs;

public record UserResponse(
    Guid UserId,
    string Name,
    string PhoneNumber,
    string EmailAddress);

