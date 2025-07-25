using System;

namespace PropertyListingAPI.DTOs;

public class ViewingRequestCreateDto
{
    public required int PropertyId { get; set; }
    public required DateTime ViewingDate { get; set; }
}

