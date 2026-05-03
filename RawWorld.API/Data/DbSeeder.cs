namespace RawWorld.API.Data;

using BCrypt.Net;
using RawWorld.API.Models;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category { Name = "Juices" },
                new Category { Name = "Smoothies" },
                new Category { Name = "Detox Drinks" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Users.Any(u => u.Role == "Admin"))
        {
            db.Users.Add(new User
            {
                FullName = "Admin User",
                Email = "admin@rawworld.com",
                PasswordHash = BCrypt.HashPassword("Admin@123"),
                Role = "Admin"
            });
            await db.SaveChangesAsync();
        }
    }
}