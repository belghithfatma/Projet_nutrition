using Microsoft.EntityFrameworkCore;
using app_nutri.Models;
using app_nutri.Services;

namespace app_nutri.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        bool adminExists = await context.Users
            .AnyAsync(u => u.Email == "admin@nutrition.com");

        if (!adminExists)
        {
            var admin = new User
            {
                FullName = "Admin",
                Email = "admin@nutrition.com",
                PasswordHash = PasswordHasher.HashPassword("Admin123*"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}