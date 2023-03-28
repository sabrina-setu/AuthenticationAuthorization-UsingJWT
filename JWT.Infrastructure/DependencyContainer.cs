using JWT.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWT.Infrastructure;

public static class DependencyContainer
{
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDBContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("JWTConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName)), ServiceLifetime.Scoped);
    }
}
