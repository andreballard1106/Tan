using MediatR;
using Tandem.Application.DTOs;
using Tandem.Application.Queries;
using Tandem.Domain.Repositories;

namespace Tandem.Application.Handlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserResponse?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByEmailQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<UserResponse?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAddressAsync(
            request.EmailAddress, 
            cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserResponse(
            user.UserId,
            user.GetFullName(),
            user.PhoneNumber,
            user.EmailAddress);
    }
}

