using PropertyListingAPI.Interfaces;

namespace PropertyListingAPI.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // TODO: Implement actual email sending (e.g., using SendGrid, SMTP, etc.)
        // For now, just log the email details
        _logger.LogInformation($"Email would be sent to: {to}");
        _logger.LogInformation($"Subject: {subject}");
        _logger.LogInformation($"Body: {body}");
        
        // Simulate async operation
        await Task.CompletedTask;
    }
} 