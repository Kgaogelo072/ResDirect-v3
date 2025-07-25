using System;
using PropertyListingAPI.Models;

namespace PropertyListingAPI.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}

