﻿using JWT.Api.RequestModels.Account;
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

            var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
            var isMailSent = await emailService.SendEmailAsync(signUpRequest.Email, "Reset Password", $"<a href='{confirmationLink}'>Please click this link to reset password.</a>");

            return isMailSent ? Ok("Please check you email inbox, an email is sent.") : Ok("Can't send email.");

            //return Ok(token);
        }

       return BadRequest();
    }
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInRequestModel signInRequest)
    {
        var user = await userManager.FindByEmailAsync(signInRequest.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, signInRequest.Password)) 
            return BadRequest("Email or Password is not correct!");
        var token = tokenGenerator.GenerateToken(user);
        return Ok(token);
    }
}
