using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required bool IsApproved { get; set; } = false;
    public UserRole Role { get; set; } = UserRole.Tenant;
    public ICollection<Property> Properties { get; set; } = new List<Property>();
}


