using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(ApplicationDbContext context, IConfiguration configuration)
    {
        var email = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"];
        bool adminExists = await context.Users.AnyAsync(u => u.Role == "Admin");

        if (adminExists)
        {
            return;
        }

        User admin = new()
        {
            Email = "admin@placementportal.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword!),
            Role = "Admin",
            IsVerified = true
        };

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}