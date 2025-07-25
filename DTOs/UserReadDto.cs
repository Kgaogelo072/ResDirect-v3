using System;

namespace PropertyListingAPI.DTOs;

public class UserReadDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required bool IsApproved { get; set; }
}
