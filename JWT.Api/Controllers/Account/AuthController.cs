using JWT.Api.RequestModels.Account;
using JWT.Application.UseCase.Account;
using JWT.Core.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Api.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ITokenGenerator tokenGenerator;
    public AuthController(UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator)
    {
        this.userManager = userManager;
        this.tokenGenerator = tokenGenerator;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpRequestModel signUpRequest)
    {
        var user = new ApplicationUser()
        {
            UserName = signUpRequest.Email,
            Email = signUpRequest.Email,
            FirstName = signUpRequest.FirstName,
            LastName = signUpRequest.LastName,
        };
        var result = await userManager.CreateAsync(user, signUpRequest.Password);
        var token = tokenGenerator.GenerateToken(user);
        if (result.Succeeded) return Ok(token);
        return BadRequest();
    }
}
