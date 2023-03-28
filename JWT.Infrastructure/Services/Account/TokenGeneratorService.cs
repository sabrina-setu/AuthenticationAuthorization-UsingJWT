using IdentityModel;
using JWT.Application.UseCase.Account;
using JWT.Core.Common;
using JWT.Core.Entities.Auth;
using JWT.Infrastructure.DBContexts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT.Infrastructure.Services.Account;

internal class TokenGeneratorService : ITokenGenerator
{
    private readonly ApplicationDBContext applicationDBContext;
    private readonly AuthConfigs authConfigs;
    public TokenGeneratorService (ApplicationDBContext applicationDBContext,IOptions<AuthConfigs> options)
    {
        this.applicationDBContext = applicationDBContext;
        this.authConfigs = options.Value;
    }
    public string GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtClaimTypes.Id,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfigs.JwtSecretKey));

        var token = new JwtSecurityToken(
            issuer: authConfigs.Issuer,
            audience: authConfigs.Audience,
            expires: DateTime.Now.AddHours(authConfigs.ExpireTime),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
            );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}
