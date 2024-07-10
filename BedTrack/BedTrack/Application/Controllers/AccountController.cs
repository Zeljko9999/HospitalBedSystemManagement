using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BedTrack.Domain.Models;
using BedTrack.Application.NewDTO;
using BedTrack.Application.DTO;
using BedTrack.Domain.Exceptions;
using BedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BedTrack.DAL.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FactoryApplication.Filters;

namespace BedTrack.Application.Controllers
{
    [ErrorFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserLogic _userLogic;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
            SignInManager<User> signInManager, IUserLogic userLogic, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userLogic = userLogic;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDTO model)
        {
            try
            {
                await _userLogic.ValidateEmailField(model.Email);
                await _userLogic.ValidateNameField(model.Name);

                // Convert input string fields into first letter as upppercase and all other into lowercase
                if (!(string.IsNullOrEmpty(model.Name)))
                {
                    var words = model.Name.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                    }
                    model.Name = string.Join(' ', words);
                }

                var user = model.ToModel();

                // Create user
                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    _logger.LogInformation("User created successfully.");

                    // Retrieve the created user
                    var savedUser = await _userManager.FindByNameAsync(user.UserName);
                    if (savedUser != null)
                    {
                        _logger.LogInformation($"Retrieved user ID: {savedUser.Id}");

                        // Check if the role exists
                        if (!await _roleManager.RoleExistsAsync(model.Role))
                        {
                            _logger.LogInformation($"Role '{model.Role}' does not exist. Creating it.");
                            var newRole = new IdentityRole<int> { Name = model.Role };
                            var roleResult = await _roleManager.CreateAsync(newRole);
                            if (!roleResult.Succeeded)
                            {
                                return BadRequest("Failed to create role.");
                            }
                        }

                        // Assign role to user
                        var role = await _roleManager.FindByNameAsync(model.Role);
                        var addToRoleResult = await _userManager.AddToRoleAsync(savedUser, role.Name);
                        if (!addToRoleResult.Succeeded)
                        {
                            return BadRequest("Failed to assign role to user.");
                        }

                        // Add claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, savedUser.Id.ToString()),
                            new Claim(ClaimTypes.Role, role.Name),
                        };

                        var addClaimsResult = await _userManager.AddClaimsAsync(savedUser, claims);
                        if (!addClaimsResult.Succeeded)
                        {
                            return BadRequest("Failed to add claims to user.");
                        }

                        return Ok(new { message = "User registered successfully." });
                    }
                    else
                    {
                        _logger.LogError("Failed to retrieve the saved user.");
                        return BadRequest(new { message = "Saved user not found." });
                    }
                }
                else
                {
                    _logger.LogError("Failed to create user.");
                    return BadRequest(createResult.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during user registration.");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete(".AspNetCore.Identity.Application", new CookieOptions
            {
                Path = "/",
                SameSite = SameSiteMode.None,
                Secure = true
            });
            return Ok(new { message = "Logged out successfully" });
        }
    }
}