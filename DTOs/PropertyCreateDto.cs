using System;

namespace PropertyListingAPI.DTOs;

public class PropertyCreateDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal RentalAmount { get; set; }
    public required string Address { get; set; }
    public required int Bedrooms { get; set; }
    public required int Bathrooms { get; set; }
    public required string ImageUrl { get; set; }
}

