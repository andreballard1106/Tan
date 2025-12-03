using Microsoft.EntityFrameworkCore;
using Tandem.Domain.Entities;
using Tandem.Domain.Repositories;
using Tandem.Infrastructure.Data;

namespace Tandem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TandemDbContext _context;

    public UserRepository(TandemDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            throw new ArgumentException("Email address cannot be null or empty.", nameof(emailAddress));
        }

        return await _context.Users
            .FirstOrDefaultAsync(u => u.EmailAddress == emailAddress, cancellationToken);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<bool> ExistsByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            return false;
        }

        return await _context.Users
            .AnyAsync(u => u.EmailAddress == emailAddress, cancellationToken);
    }
}

