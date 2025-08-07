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
    
    // Multiple images support (up to 5 images)
    public List<IFormFile> Images { get; set; } = new();
    public List<int> ImageOrders { get; set; } = new(); // Display order for each image
    public int PrimaryImageIndex { get; set; } = 0; // Which image is primary (0-based index)
}

