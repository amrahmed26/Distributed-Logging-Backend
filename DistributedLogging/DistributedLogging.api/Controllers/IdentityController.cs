using DistributedLogging.api.Auth.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using DistributedLogging.Web.Api.Auth.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace DistributedLogging.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtHandler _jwtHandler;
        public IdentityController(UserManager<IdentityUser> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
        {
            var user =await _userManager.FindByEmailAsync(requestModel.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user,requestModel.Password))
                return BadRequest(new LoginResponseModel { IsSucceeded = false, ErrorMessage = "User Name OR Password Is Not Valid" });

            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new LoginResponseModel { IsSucceeded = true, Token = token });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Optionally sign in the user immediately after registration

                    return Ok(new { message = "User registered successfully." });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Invalid model state.");
        }
    }
}
