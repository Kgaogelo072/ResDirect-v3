using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyListingAPI.DTOs;
using PropertyListingAPI.Interfaces; 
using PropertyListingAPI.Models;
using PropertyListingAPI.Data;


namespace PropertyListingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserRegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email.ToLower()))
            return BadRequest("Email already registered");

        using var hmac = new HMACSHA512();

        var user = new User
        {
            FullName = $"{dto.FirstName} {dto.LastName}",
            Email = dto.Email.ToLower(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key,
            IsApproved = false,
            Role = dto.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = user.PhoneNumber,
            Token = _tokenService.CreateToken(user),
            Role = user.Role,
            IsApproved = user.IsApproved
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email.ToLower());
        if (user == null) return Unauthorized("Invalid email");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
            return Unauthorized("Invalid password");

        // Split the full name into first and last name
        var nameParts = user.FullName.Split(' ', 2);
        var firstName = nameParts.Length > 0 ? nameParts[0] : "";
        var lastName = nameParts.Length > 1 ? nameParts[1] : "";

        return Ok(new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = user.PhoneNumber,
            Token = _tokenService.CreateToken(user),
            Role = user.Role,
            IsApproved = user.IsApproved
        });
    }
}

