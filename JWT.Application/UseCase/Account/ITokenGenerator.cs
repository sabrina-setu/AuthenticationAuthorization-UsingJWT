using JWT.Core.Entities.Auth;

namespace JWT.Application.UseCase.Account;

public interface ITokenGenerator
{
    public string GenerateToken(ApplicationUser user);
}
