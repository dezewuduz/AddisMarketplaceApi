using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AddisMarketplaceApi.Data;
using AddisMarketplaceApi.Models;

namespace AddisMarketplaceApi.Controllers;

public class RegisterDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<Seller>> Register(RegisterDto dto)
    {
        var exists = await _context.Sellers.AnyAsync(s => s.PhoneNumber == dto.PhoneNumber);
        if (exists) return BadRequest("ይህ ስልክ ቁጥር ቀድሞ ተመዝግቧል።");

        var seller = new Seller
        {
            Name = dto.Name,
            Location = dto.Location,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsVerified = false,
            IsSubscribed = false
        };

        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        return Ok(new { message = "ተመዝግቧል", sellerId = seller.Id });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.PhoneNumber == dto.PhoneNumber);
        if (seller == null || !BCrypt.Net.BCrypt.Verify(dto.Password, seller.PasswordHash))
            return Unauthorized("ስልክ ቁጥር ወይም የይለፍ ቃል ልክ አይደለም።");

        var token = GenerateJwtToken(seller);
        return Ok(new { token, sellerId = seller.Id, name = seller.Name });
    }

    private string GenerateJwtToken(Seller seller)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, seller.Id.ToString()),
            new Claim(ClaimTypes.Name, seller.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}