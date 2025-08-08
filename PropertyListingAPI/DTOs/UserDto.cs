using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string FullName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsApproved { get; set; }
}
