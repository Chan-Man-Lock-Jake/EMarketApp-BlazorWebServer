using System.IdentityModel.Tokens.Jwt;  // JWT
using System.Security.Claims;           // Claims
using System.Text;                      // Encoding
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;   // Token Algorithmns
using EMarketApp.Data;
using EMarketApp.Models;

namespace EMarketApp.Services;

public class AuthService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public AuthService(IDbContextFactory<ApplicationDbContext> dbContextFactory, ApplicationDbContext context, ILogger<AuthService> logger, UserManager<User> manager)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = context;
        _logger = logger;
        _userManager = manager;
    }

    // (DONE) POST /api/register – Register new user
    public async Task<IdentityResult> RegisterUserAsync(string username, string email, string password)
    {
        // Check if username already exists
        var existingUsername = await _userManager.FindByNameAsync(username);
        if (existingUsername != null)
        {
            return IdentityResult.Failed(new IdentityError { 
                Code = "DuplicateUserName",
                Description = "Username is already taken." 
            });
        }

        // Check if email already exists
        var existingEmail = await _userManager.FindByEmailAsync(email);
        if (existingEmail != null)
        {
            return IdentityResult.Failed(new IdentityError {
                Code = "DuplicateEmail",
                Description = "Email is already taken."
            });
        }

        // Create new user
        var user = new User { UserName = username, Email = email };
        return await _userManager.CreateAsync(user, password);
    }
    
    //  POST /api/login – Login, return token/session
    // public async Task<LoginResult> LoginUserAsync(LoginRequest request)
    // {
    //     var user = await _userManager.FindByNameAsync(request.UserName);
    //     if (user == null)
    //     {
    //         return new LoginResult { Success = false, Error = "Invalid username or password" };
    //     }

    //     var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
    //     if (!result.Succeeded)
    //     {
    //         return new LoginResult { Success = false, Error = "Invalid username or password" };
    //     }

    //     var token = GenerateJwtToken(user);
    //     return new LoginResult { Success = true, Token = token };
    // }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //  POST /api/logout – Logout (optional)

    public async Task ClearAllUsersAsync()
    {
        var users = _userManager.Users.ToList();

        foreach (var user in users)
        {
            await _userManager.DeleteAsync(user);
        }
    }

    public async Task<bool> UsernameExistAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return false;
        }
        _logger.LogInformation("UserName exists.");
        return true;
    }
    public async Task<bool> EmailExistAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }
        _logger.LogInformation("Email exists.");
        return true;
    }
}
