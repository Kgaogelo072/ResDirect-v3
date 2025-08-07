using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.DTOs;

public class UserRegisterDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Agent;
}

