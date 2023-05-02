using JWT.Api.RequestModels.Account;
using JWT.Application.UseCase.Account;
using JWT.Core.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace JWT.Api.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ITokenGenerator tokenGenerator;
    private readonly IEmailService emailService;
    public AuthController(UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator, IEmailService emailService)
    {
        this.userManager = userManager;
        this.tokenGenerator = tokenGenerator;
        this.emailService = emailService;
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
        //var token = tokenGenerator.GenerateToken(user);
        if (result.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = $"{signUpRequest.Email}/{token}";
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

           // var confirmationLink = nameof(ConfirmEmail) + "confirmEmail?token=" + token;
           var confirmationLink = "https://localhost:7175/swagger/index.html";
            var isMailSent = await emailService.SendEmailAsync(signUpRequest.Email, "Confirm Email", $"<a href='{confirmationLink}'>Please click this link to confirm email.</a>");

            return isMailSent ? Ok("Please check you email inbox, an email is sent.") : Ok("Can't send email.");

            //return Ok(token);
        }

       return BadRequest();
    }
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return BadRequest("Error");

        var result = await userManager.ConfirmEmailAsync(user, token);
        return Ok(result.Succeeded ? nameof(ConfirmEmail) : "Error");
    }
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInRequestModel signInRequest)
    {
        var user = await userManager.FindByEmailAsync(signInRequest.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, signInRequest.Password) || user.EmailConfirmed != true) 
            return BadRequest("Email or Password is not correct!");
        var token = tokenGenerator.GenerateToken(user);
        return Ok(token);
    }
}
