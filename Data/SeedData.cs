using PropertyListingAPI.Models;
using PropertyListingAPI.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace PropertyListingAPI.Data;

public static class SeedData
{
    public static void SeedUsers(ModelBuilder builder)
    {
       builder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            FullName = "Kgaogelo Tshabalala",
            Email = "Fortunatekgaogelo@gmail.com",
            PasswordHash = Convert.FromBase64String("7Re9Lo4rvoU+eL8NuDfO1FwUsyXyDXoJXBX1X8avFJbdtvKU4b3EcfJ3WrkHKkoXg63MLaYi4VLOXUDGwVLzKQ=="),
            PasswordSalt = Convert.FromBase64String("Bxf0U9e+jlwA5sJY3+UmG9s8tbKMoQo3gZy4Ftf2gmhrPe7rmZUrRSxN4n+vTcb4zdxYBCZnUR7D4Jzwk4OXjw=="),
            Role = UserRole.Admin,
            PhoneNumber = "0728945924",
            IsApproved = true
        },
        new User
        {
            Id = 2,
            FullName = "Agent User",
            Email = "agent@example.com",
            PasswordHash = Convert.FromBase64String("7Re9Lo4rvoU+eL8NuDfO1FwUsyXyDXoJXBX1X8avFJbdtvKU4b3EcfJ3WrkHKkoXg63MLaYi4VLOXUDGwVLzKQ=="),
            PasswordSalt = Convert.FromBase64String("Bxf0U9e+jlwA5sJY3+UmG9s8tbKMoQo3gZy4Ftf2gmhrPe7rmZUrRSxN4n+vTcb4zdxYBCZnUR7D4Jzwk4OXjw=="),
            Role = UserRole.Agent,
            PhoneNumber = "0728945924",
            IsApproved = true
        },
        new User
        {
            Id = 3,
            FullName = "Tenant User",
            Email = "tenant@example.com",
            PasswordHash = Convert.FromBase64String("7Re9Lo4rvoU+eL8NuDfO1FwUsyXyDXoJXBX1X8avFJbdtvKU4b3EcfJ3WrkHKkoXg63MLaYi4VLOXUDGwVLzKQ=="),
            PasswordSalt = Convert.FromBase64String("Bxf0U9e+jlwA5sJY3+UmG9s8tbKMoQo3gZy4Ftf2gmhrPe7rmZUrRSxN4n+vTcb4zdxYBCZnUR7D4Jzwk4OXjw=="),
            Role = UserRole.Tenant,
            PhoneNumber = "0728945924",
            IsApproved = false
        });

    }
}
