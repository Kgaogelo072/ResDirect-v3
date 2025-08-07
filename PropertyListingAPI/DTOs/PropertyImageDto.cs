using System;

namespace PropertyListingAPI.DTOs;

public class PropertyImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
} 