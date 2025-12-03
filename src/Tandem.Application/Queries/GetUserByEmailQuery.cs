using MediatR;
using Tandem.Application.DTOs;

namespace Tandem.Application.Queries;

public record GetUserByEmailQuery(string EmailAddress) : IRequest<UserResponse?>;

