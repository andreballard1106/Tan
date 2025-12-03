using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tandem.Domain.Repositories;
using Tandem.Infrastructure.Data;
using Tandem.Infrastructure.Repositories;

namespace Tandem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<TandemDbContext>(options =>
            options.UseInMemoryDatabase("TandemDb"));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

