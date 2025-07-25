using System;

namespace PropertyListingAPI.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
