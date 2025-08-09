using PropertyListingAPI.Interfaces;
using System.Text;
using System.Text.Json;

namespace PropertyListingAPI.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WhatsAppService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string? _accessToken;
    private readonly string? _phoneNumberId;
    private readonly string? _apiVersion;

    public WhatsAppService(HttpClient httpClient, ILogger<WhatsAppService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        
        // Get WhatsApp configuration from appsettings
        _accessToken = _configuration["WhatsAppSettings:AccessToken"];
        _phoneNumberId = _configuration["WhatsAppSettings:PhoneNumberId"];
        _apiVersion = _configuration["WhatsAppSettings:ApiVersion"] ?? "v18.0";
        
        // Configure HttpClient
        _httpClient.BaseAddress = new Uri("https://graph.facebook.com/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task SendMessageAsync(string toPhoneNumber, string message)
    {
        if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_phoneNumberId))
        {
            _logger.LogWarning("WhatsApp configuration is missing. Access Token or Phone Number ID not configured.");
            _logger.LogInformation($"Would send WhatsApp message to: {toPhoneNumber}");
            _logger.LogInformation($"Message: {message}");
            return;
        }

        try
        {
            var requestData = new
            {
                messaging_product = "whatsapp",
                to = FormatPhoneNumber(toPhoneNumber),
                type = "text",
                text = new { body = message }
            };

            var jsonContent = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var endpoint = $"{_apiVersion}/{_phoneNumberId}/messages";
            _logger.LogInformation($"Sending WhatsApp message to {toPhoneNumber} via endpoint: {endpoint}");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"WhatsApp message sent successfully to {toPhoneNumber}");
                _logger.LogDebug($"WhatsApp API Response: {responseContent}");
            }
            else
            {
                _logger.LogError($"Failed to send WhatsApp message to {toPhoneNumber}. Status: {response.StatusCode}");
                _logger.LogError($"WhatsApp API Error Response: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception occurred while sending WhatsApp message to {toPhoneNumber}");
        }
    }

    public async Task SendTemplateMessageAsync(string toPhoneNumber, string templateName, string[] parameters)
    {
        if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_phoneNumberId))
        {
            _logger.LogWarning("WhatsApp configuration is missing. Access Token or Phone Number ID not configured.");
            _logger.LogInformation($"Would send WhatsApp template message to: {toPhoneNumber}");
            _logger.LogInformation($"Template: {templateName}, Parameters: {string.Join(", ", parameters)}");
            return;
        }

        try
        {
            var templateParameters = parameters.Select(param => new { type = "text", text = param }).ToArray();

            var requestData = new
            {
                messaging_product = "whatsapp",
                to = FormatPhoneNumber(toPhoneNumber),
                type = "template",
                template = new
                {
                    name = templateName,
                    language = new { code = "en_US" },
                    components = new[]
                    {
                        new
                        {
                            type = "body",
                            parameters = templateParameters
                        }
                    }
                }
            };

            var jsonContent = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var endpoint = $"{_apiVersion}/{_phoneNumberId}/messages";
            _logger.LogInformation($"Sending WhatsApp template message to {toPhoneNumber}");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"WhatsApp template message sent successfully to {toPhoneNumber}");
                _logger.LogDebug($"WhatsApp API Response: {responseContent}");
            }
            else
            {
                _logger.LogError($"Failed to send WhatsApp template message to {toPhoneNumber}. Status: {response.StatusCode}");
                _logger.LogError($"WhatsApp API Error Response: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception occurred while sending WhatsApp template message to {toPhoneNumber}");
        }
    }

    public async Task SendMediaMessageAsync(string toPhoneNumber, string mediaUrl, string mediaType, string? caption = null)
    {
        if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_phoneNumberId))
        {
            _logger.LogWarning("WhatsApp configuration is missing. Access Token or Phone Number ID not configured.");
            _logger.LogInformation($"Would send WhatsApp media message to: {toPhoneNumber}");
            _logger.LogInformation($"Media URL: {mediaUrl}, Type: {mediaType}, Caption: {caption}");
            return;
        }

        try
        {
            var mediaObject = new Dictionary<string, object>
            {
                { "link", mediaUrl }
            };

            if (!string.IsNullOrEmpty(caption))
            {
                mediaObject.Add("caption", caption);
            }

            var requestData = new
            {
                messaging_product = "whatsapp",
                to = FormatPhoneNumber(toPhoneNumber),
                type = mediaType.ToLower(),
            };

            // Add the media object with the appropriate key based on media type
            var requestDict = new Dictionary<string, object>
            {
                { "messaging_product", "whatsapp" },
                { "to", FormatPhoneNumber(toPhoneNumber) },
                { "type", mediaType.ToLower() },
                { mediaType.ToLower(), mediaObject }
            };

            var jsonContent = JsonSerializer.Serialize(requestDict, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var endpoint = $"{_apiVersion}/{_phoneNumberId}/messages";
            _logger.LogInformation($"Sending WhatsApp {mediaType} message to {toPhoneNumber}");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"WhatsApp {mediaType} message sent successfully to {toPhoneNumber}");
                _logger.LogDebug($"WhatsApp API Response: {responseContent}");
            }
            else
            {
                _logger.LogError($"Failed to send WhatsApp {mediaType} message to {toPhoneNumber}. Status: {response.StatusCode}");
                _logger.LogError($"WhatsApp API Error Response: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception occurred while sending WhatsApp {mediaType} message to {toPhoneNumber}");
        }
    }

    public async Task SendLocationMessageAsync(string toPhoneNumber, double latitude, double longitude, string? name = null, string? address = null)
    {
        if (string.IsNullOrEmpty(_accessToken) || string.IsNullOrEmpty(_phoneNumberId))
        {
            _logger.LogWarning("WhatsApp configuration is missing. Access Token or Phone Number ID not configured.");
            _logger.LogInformation($"Would send WhatsApp location message to: {toPhoneNumber}");
            _logger.LogInformation($"Location: {latitude}, {longitude}, Name: {name}, Address: {address}");
            return;
        }

        try
        {
            var locationObject = new Dictionary<string, object>
            {
                { "latitude", latitude },
                { "longitude", longitude }
            };

            if (!string.IsNullOrEmpty(name))
                locationObject.Add("name", name);

            if (!string.IsNullOrEmpty(address))
                locationObject.Add("address", address);

            var requestData = new
            {
                messaging_product = "whatsapp",
                to = FormatPhoneNumber(toPhoneNumber),
                type = "location",
                location = locationObject
            };

            var jsonContent = JsonSerializer.Serialize(requestData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var endpoint = $"{_apiVersion}/{_phoneNumberId}/messages";
            _logger.LogInformation($"Sending WhatsApp location message to {toPhoneNumber}");

            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"WhatsApp location message sent successfully to {toPhoneNumber}");
                _logger.LogDebug($"WhatsApp API Response: {responseContent}");
            }
            else
            {
                _logger.LogError($"Failed to send WhatsApp location message to {toPhoneNumber}. Status: {response.StatusCode}");
                _logger.LogError($"WhatsApp API Error Response: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception occurred while sending WhatsApp location message to {toPhoneNumber}");
        }
    }

    public async Task<bool> ValidateWebhookAsync(string mode, string token, string challenge)
    {
        var verifyToken = _configuration["WhatsAppSettings:VerifyToken"];
        
        if (mode == "subscribe" && token == verifyToken)
        {
            _logger.LogInformation("WhatsApp webhook validated successfully");
            return true;
        }
        
        _logger.LogWarning("WhatsApp webhook validation failed");
        return false;
    }

    private string FormatPhoneNumber(string phoneNumber)
    {
        // Remove any non-digit characters except the leading +
        var cleaned = phoneNumber.Trim();
        
        // If it starts with +, remove it (WhatsApp API expects numbers without +)
        if (cleaned.StartsWith("+"))
        {
            cleaned = cleaned.Substring(1);
        }
        
        // If it starts with 0, remove it and add country code (assuming South African numbers)
        if (cleaned.StartsWith("0"))
        {
            cleaned = "27" + cleaned.Substring(1);
        }
        
        return cleaned;
    }
} 