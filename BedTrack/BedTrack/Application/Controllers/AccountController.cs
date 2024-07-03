using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging; // Import the logging namespace
using System.Threading.Tasks;
using BedTrack.Domain.Models;
using BedTrack.Application.NewDTO;
using BedTrack.Application.DTO;
using BedTrack.Domain.Exceptions;
using BedTrack.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BedTrack.DAL.Data;
using System.Security.Claims;

namespace BedTrack.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserLogic _userLogic;
        private readonly BedTrackContext db;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager,
            SignInManager<User> signInManager, IUserLogic userLogic, ILogger<AccountController> logger, BedTrackContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userLogic = userLogic;
            _logger = logger;
            db = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDTO model)
        {
            try
            {
                await _userLogic.ValidateEmailField(model.Email);
                await _userLogic.ValidateNameField(model.Name);

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

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            try
            {
                _logger.LogInformation("Finding user by email.");
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    _logger.LogInformation("User found. Attempting to sign in.");
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return Ok(new { message = "Logged in successfully" });
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return BadRequest(new { message = "Requires two-factor authentication" });
                    }

                    if (result.IsLockedOut)
                    {
                        return BadRequest(new { message = "User account locked out" });
                    }
                }
                else
                {
                    return BadRequest(new { message = "User not found" });
                }

                return BadRequest(new { message = "Invalid login attempt" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error during login.");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}