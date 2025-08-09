using System;

namespace PropertyListingAPI.Interfaces;

public interface IWhatsAppService
{
    /// <summary>
    /// Send a simple text message to a WhatsApp number
    /// </summary>
    Task SendMessageAsync(string toPhoneNumber, string message);
    
    /// <summary>
    /// Send a template message with parameters
    /// </summary>
    Task SendTemplateMessageAsync(string toPhoneNumber, string templateName, string[] parameters);
    
    /// <summary>
    /// Send a media message (image, video, audio, document)
    /// </summary>
    Task SendMediaMessageAsync(string toPhoneNumber, string mediaUrl, string mediaType, string? caption = null);
    
    /// <summary>
    /// Send a location message
    /// </summary>
    Task SendLocationMessageAsync(string toPhoneNumber, double latitude, double longitude, string? name = null, string? address = null);
    
    /// <summary>
    /// Validate WhatsApp webhook verification
    /// </summary>
    Task<bool> ValidateWebhookAsync(string mode, string token, string challenge);
}
