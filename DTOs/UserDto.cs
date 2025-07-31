using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.DTOs;

public class UserDto
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string FullName { get; set; }
    public UserRole Role { get; set; }
}
