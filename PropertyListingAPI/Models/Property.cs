using System;

namespace PropertyListingAPI.Models;

public class Property
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal RentalAmount { get; set; }
    public required string Address { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public required string ImageUrl { get; set; }
    public required string ImagePublicId { get; set; }
    public int AgentId { get; set; }
    public User? Agent { get; set; }
}

