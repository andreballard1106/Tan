using MediatR;
using Tandem.Application.Commands;
using Tandem.Application.DTOs;
using Tandem.Domain.Entities;
using Tandem.Domain.Repositories;

namespace Tandem.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAddressAsync(
            request.Request.EmailAddress, 
            cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with email address '{request.Request.EmailAddress}' already exists.");
        }

        var user = new User(
            request.Request.FirstName,
            request.Request.MiddleName,
            request.Request.LastName,
            request.Request.PhoneNumber,
            request.Request.EmailAddress);

        var createdUser = await _userRepository.AddAsync(user, cancellationToken);

        return new UserResponse(
            createdUser.UserId,
            createdUser.GetFullName(),
            createdUser.PhoneNumber,
            createdUser.EmailAddress);
    }
}

