using MediatR;
using Tandem.Application.DTOs;

namespace Tandem.Application.Commands;

public record CreateUserCommand(CreateUserRequest Request) : IRequest<UserResponse>;

