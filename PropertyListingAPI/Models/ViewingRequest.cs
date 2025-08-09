using System;
using PropertyListingAPI.Enums;

namespace PropertyListingAPI.Models;

public class ViewingRequest
{
    public int Id { get; set; }

    public int PropertyId { get; set; }
    public Property? Property { get; set; }

    // For registered users (optional - can be null for guest requests)
    public int? TenantId { get; set; }
    public User? Tenant { get; set; }

    // Guest information (for non-registered users)
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }

    public DateTime ViewingDate { get; set; } = DateTime.UtcNow;
    public string? PreferredTime { get; set; }
    public string? Message { get; set; }

    public ViewingStatus Status { get; set; } = ViewingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Helper property to get requester name (either from User or Guest)
    public string RequesterName => Tenant?.FullName ?? GuestName ?? "Unknown";
    public string RequesterEmail => Tenant?.Email ?? GuestEmail ?? "Unknown";
    public string RequesterPhone => Tenant?.PhoneNumber ?? GuestPhone ?? "Unknown";
}

