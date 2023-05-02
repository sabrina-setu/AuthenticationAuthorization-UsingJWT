using JWT.Application.UseCase.Account;
using JWT.Core.Entities.Auth;
using JWT.Infrastructure.DBContexts;
using JWT.Infrastructure.Services.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JWT.Infrastructure;

public static class DependencyContainer
{
    public static void AddAuthenticationDependencies(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            //opt.SignIn.RequireConfirmedEmail = false;
            opt.SignIn.RequireConfirmedPhoneNumber = false;
            opt.SignIn.RequireConfirmedAccount = false;
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = true;

            opt.Password.RequiredUniqueChars = 0;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();
    }
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDBContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("JWTConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName)), ServiceLifetime.Scoped);

        services.AddScoped<ITokenGenerator, TokenGeneratorService>();
        services.AddScoped<IEmailService, EmailService>();
    }
}
