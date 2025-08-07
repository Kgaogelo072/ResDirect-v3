using System;

namespace PropertyListingAPI.DTOs;

public class PropertyReadDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal RentalAmount { get; set; }
    public required string Address { get; set; }
    public required int Bedrooms { get; set; }
    public required int Bathrooms { get; set; }
    public required string AgentName { get; set; }
    
    // Multiple images support
    public List<PropertyImageDto> Images { get; set; } = new();
    public string PrimaryImageUrl { get; set; } = string.Empty; // For quick access to main image
}

