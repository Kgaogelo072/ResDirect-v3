using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.Models;

public class ViewingRequest
{
    public int Id { get; set; }

    public int PropertyId { get; set; }
    public Property? Property { get; set; }

    public int TenantId { get; set; }
    public User? Tenant { get; set; }

    public DateTime ViewingDate { get; set; } = DateTime.UtcNow;

    public ViewingStatus Status { get; set; } = ViewingStatus.Pending;
}

