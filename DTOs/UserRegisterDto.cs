using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.DTOs;

public class UserRegisterDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Tenant;
}

