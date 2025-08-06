using System;

namespace PropertyListingAPI.Interfaces;

public interface IWhatsAppService
{
    Task SendMessageAsync(string toPhoneNumber, string message);
}
