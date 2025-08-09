using System.ComponentModel.DataAnnotations;

namespace PropertyListingAPI.DTOs;

public class GuestViewingRequestDto
{
    [Required]
    public int PropertyId { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string GuestName { get; set; }
    
    [Required]
    [EmailAddress]
    public required string GuestEmail { get; set; }
    
    [Required]
    [Phone]
    public required string GuestPhone { get; set; }
    
    [Required]
    public DateTime PreferredDate { get; set; }
    
    [Required]
    public required string PreferredTime { get; set; }
    
    public string? Message { get; set; }
} 