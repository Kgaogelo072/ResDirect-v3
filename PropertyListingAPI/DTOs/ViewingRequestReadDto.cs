using System;

namespace PropertyListingAPI.DTOs;

public class ViewingRequestReadDto
{
    public int Id { get; set; }
    public required string PropertyTitle { get; set; }
    public required string TenantName { get; set; }
    public required DateTime ViewingDate { get; set; }
    public required string Status { get; set; }
}

