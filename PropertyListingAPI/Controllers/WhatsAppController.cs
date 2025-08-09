using Microsoft.AspNetCore.Mvc;
using PropertyListingAPI.Interfaces;
using System.Text.Json;

namespace PropertyListingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService;
    private readonly ILogger<WhatsAppController> _logger;

    public WhatsAppController(IWhatsAppService whatsAppService, ILogger<WhatsAppController> logger)
    {
        _whatsAppService = whatsAppService;
        _logger = logger;
    }

    /// <summary>
    /// Webhook verification endpoint for WhatsApp
    /// </summary>
    [HttpGet("webhook")]
    public async Task<IActionResult> VerifyWebhook(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.challenge")] string challenge,
        [FromQuery(Name = "hub.verify_token")] string verifyToken)
    {
        _logger.LogInformation($"WhatsApp webhook verification request: mode={mode}, token={verifyToken}");

        var isValid = await _whatsAppService.ValidateWebhookAsync(mode, verifyToken, challenge);
        
        if (isValid)
        {
            _logger.LogInformation("WhatsApp webhook verification successful");
            return Ok(challenge);
        }
        
        _logger.LogWarning("WhatsApp webhook verification failed");
        return Unauthorized("Invalid verification token");
    }

    /// <summary>
    /// Webhook endpoint to receive WhatsApp messages and events
    /// </summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> ReceiveWebhook([FromBody] JsonDocument webhookData)
    {
        try
        {
            _logger.LogInformation("Received WhatsApp webhook");
            _logger.LogDebug($"Webhook data: {webhookData.RootElement}");

            // Process the webhook data
            await ProcessWebhookData(webhookData);

            return Ok("Webhook received");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing WhatsApp webhook");
            return StatusCode(500, "Error processing webhook");
        }
    }

    /// <summary>
    /// Test endpoint to send a WhatsApp message
    /// </summary>
    [HttpPost("test-message")]
    public async Task<IActionResult> SendTestMessage([FromBody] TestMessageRequest request)
    {
        try
        {
            await _whatsAppService.SendMessageAsync(request.PhoneNumber, request.Message);
            return Ok(new { success = true, message = "Test message sent" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test WhatsApp message");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Test endpoint to send a WhatsApp template message
    /// </summary>
    [HttpPost("test-template")]
    public async Task<IActionResult> SendTestTemplate([FromBody] TestTemplateRequest request)
    {
        try
        {
            await _whatsAppService.SendTemplateMessageAsync(request.PhoneNumber, request.TemplateName, request.Parameters);
            return Ok(new { success = true, message = "Test template message sent" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test WhatsApp template message");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Test endpoint to send a WhatsApp media message
    /// </summary>
    [HttpPost("test-media")]
    public async Task<IActionResult> SendTestMedia([FromBody] TestMediaRequest request)
    {
        try
        {
            await _whatsAppService.SendMediaMessageAsync(request.PhoneNumber, request.MediaUrl, request.MediaType, request.Caption);
            return Ok(new { success = true, message = "Test media message sent" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test WhatsApp media message");
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    private async Task ProcessWebhookData(JsonDocument webhookData)
    {
        try
        {
            var entry = webhookData.RootElement.GetProperty("entry");
            
            foreach (var entryItem in entry.EnumerateArray())
            {
                if (entryItem.TryGetProperty("changes", out var changes))
                {
                    foreach (var change in changes.EnumerateArray())
                    {
                        if (change.TryGetProperty("value", out var value))
                        {
                            // Process messages
                            if (value.TryGetProperty("messages", out var messages))
                            {
                                foreach (var message in messages.EnumerateArray())
                                {
                                    await ProcessIncomingMessage(message);
                                }
                            }

                            // Process message status updates
                            if (value.TryGetProperty("statuses", out var statuses))
                            {
                                foreach (var status in statuses.EnumerateArray())
                                {
                                    await ProcessMessageStatus(status);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing webhook data");
        }
    }

    private async Task ProcessIncomingMessage(JsonElement message)
    {
        try
        {
            var from = message.GetProperty("from").GetString();
            var messageId = message.GetProperty("id").GetString();
            var timestamp = message.GetProperty("timestamp").GetString();

            _logger.LogInformation($"Received message from {from}, ID: {messageId}");

            // Handle different message types
            if (message.TryGetProperty("text", out var textMessage))
            {
                var messageBody = textMessage.GetProperty("body").GetString();
                _logger.LogInformation($"Text message: {messageBody}");
                
                // Here you can add logic to respond to specific messages
                // For example, auto-reply to certain keywords
                await HandleTextMessage(from, messageBody);
            }
            else if (message.TryGetProperty("image", out var imageMessage))
            {
                _logger.LogInformation("Received image message");
                // Handle image messages
            }
            else if (message.TryGetProperty("document", out var documentMessage))
            {
                _logger.LogInformation("Received document message");
                // Handle document messages
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing incoming message");
        }
    }

    private async Task ProcessMessageStatus(JsonElement status)
    {
        try
        {
            var messageId = status.GetProperty("id").GetString();
            var statusValue = status.GetProperty("status").GetString();
            var timestamp = status.GetProperty("timestamp").GetString();

            _logger.LogInformation($"Message {messageId} status: {statusValue}");
            
            // Here you can update your database with message delivery status
            // e.g., mark as delivered, read, failed, etc.
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message status");
        }
    }

    private async Task HandleTextMessage(string from, string messageBody)
    {
        // Simple auto-reply logic - you can expand this
        if (messageBody.ToLower().Contains("hello") || messageBody.ToLower().Contains("hi"))
        {
            await _whatsAppService.SendMessageAsync(from, 
                "Hello! Thank you for contacting ResDirect. How can we help you today?");
        }
        else if (messageBody.ToLower().Contains("properties") || messageBody.ToLower().Contains("listing"))
        {
            await _whatsAppService.SendMessageAsync(from, 
                "We have many great properties available! Please visit our website or contact our agents for more details.");
        }
        // Add more auto-reply logic as needed
    }
}

// DTOs for test endpoints
public class TestMessageRequest
{
    public required string PhoneNumber { get; set; }
    public required string Message { get; set; }
}

public class TestTemplateRequest
{
    public required string PhoneNumber { get; set; }
    public required string TemplateName { get; set; }
    public required string[] Parameters { get; set; }
}

public class TestMediaRequest
{
    public required string PhoneNumber { get; set; }
    public required string MediaUrl { get; set; }
    public required string MediaType { get; set; }
    public string? Caption { get; set; }
} 