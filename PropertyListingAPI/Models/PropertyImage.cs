using System;

namespace PropertyListingAPI.Models;

public class PropertyImage
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public required string ImageUrl { get; set; }
    public required string ImagePublicId { get; set; }
    public int DisplayOrder { get; set; } // 1-5 for ordering
    public bool IsPrimary { get; set; } // Main image for listings
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public Property Property { get; set; } = null!;
} 