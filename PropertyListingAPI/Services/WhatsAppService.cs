using PropertyListingAPI.Interfaces;

namespace PropertyListingAPI.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly ILogger<WhatsAppService> _logger;

    public WhatsAppService(ILogger<WhatsAppService> logger)
    {
        _logger = logger;
    }

    public async Task SendMessageAsync(string toPhoneNumber, string message)
    {
        // TODO: Implement actual WhatsApp messaging (e.g., using WhatsApp Business API)
        // For now, just log the message details
        _logger.LogInformation($"WhatsApp message would be sent to: {toPhoneNumber}");
        _logger.LogInformation($"Message: {message}");
        
        // Simulate async operation
        await Task.CompletedTask;
    }
} 