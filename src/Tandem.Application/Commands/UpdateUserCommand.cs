using MediatR;
using Tandem.Application.DTOs;

namespace Tandem.Application.Commands;

public record UpdateUserCommand(string EmailAddress, UpdateUserRequest Request) : IRequest<UserResponse>;

