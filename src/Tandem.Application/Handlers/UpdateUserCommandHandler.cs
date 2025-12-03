using MediatR;
using Tandem.Application.Commands;
using Tandem.Application.DTOs;
using Tandem.Domain.Repositories;

namespace Tandem.Application.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAddressAsync(
            request.EmailAddress, 
            cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with email address '{request.EmailAddress}' not found.");
        }

        user.Update(
            request.Request.FirstName,
            request.Request.MiddleName,
            request.Request.LastName,
            request.Request.PhoneNumber);

        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);

        return new UserResponse(
            updatedUser.UserId,
            updatedUser.GetFullName(),
            updatedUser.PhoneNumber,
            updatedUser.EmailAddress);
    }
}

