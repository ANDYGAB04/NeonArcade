using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeonArcade.Server.Models;

namespace NeonArcade.Server.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Create roles if they don't exist
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Get admin credentials from configuration
            var adminEmail = configuration["AdminUser:Email"];
            var adminPassword = configuration["AdminUser:Password"];
            var adminFirstName = configuration["AdminUser:FirstName"] ?? "Admin";
            var adminLastName = configuration["AdminUser:LastName"] ?? "User";

            // Validate configuration
            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException(
                    "Admin credentials not found in configuration. Please ensure AdminUser:Email and AdminUser:Password are set in appsettings.Development.json");
            }

            // Create default admin user
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                    DateOfBirth = new DateTime(1990, 1, 1)
                };

                var createAdminResult = await userManager.CreateAsync(newAdmin, adminPassword);

                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
                    var errors = string.Join(", ", createAdminResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to create admin user: {errors}");
                }
            }
        }

        public static async Task SeedGamesAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                // Check if games already exist
                var gamesCount = await context.Games.CountAsync();
                logger.LogInformation($"Current games count: {gamesCount}");

                if (gamesCount > 0)
                {
                    logger.LogInformation("Database already contains games. Skipping seed.");
                    return; // Database already seeded
                }

                logger.LogInformation("Starting to seed games...");

                var games = new List<Game>
            {
                new Game
                {
                    Title = "Cyberpunk 2077",
                    Description = "Cyberpunk 2077 is an open-world, action-adventure RPG set in the dark future of Night City — a dangerous megalopolis obsessed with power, glamor, and ceaseless body modification.",
                    ShortDescription = "Open-world action RPG in a cyberpunk dystopia",
                    Price = 59.99m,
                    DiscountPrice = 29.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co2npc.jpg",
                    Developer = "CD PROJEKT RED",
                    Publisher = "CD PROJEKT RED",
                    ReleaseDate = new DateTime(2020, 12, 10),
                    AgeRating = "18+",
                    Genres = new List<string> { "RPG", "Action", "Adventure" },
                    Platforms = new List<string> { "PC", "PlayStation", "Xbox" },
                    Tags = new List<string> { "Open World", "Cyberpunk", "Sci-Fi", "Story Rich" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 10 64-bit | CPU: Intel Core i5-3570K or AMD FX-8310 | RAM: 8 GB | GPU: NVIDIA GTX 780 or AMD Radeon RX 470",
                    RecommendedRequirements = "OS: Windows 10 64-bit | CPU: Intel Core i7-4790 or AMD Ryzen 3 3200G | RAM: 12 GB | GPU: NVIDIA GTX 1060 or AMD Radeon R9 Fury",
                    StockQuantity = 100,
                    IsAvailable = true,
                    IsFeatured = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "The Witcher 3: Wild Hunt",
                    Description = "As war rages on throughout the Northern Realms, you take on the greatest contract of your life — tracking down the Child of Prophecy, a living weapon that can alter the shape of the world.",
                    ShortDescription = "Epic fantasy RPG adventure",
                    Price = 39.99m,
                    DiscountPrice = 9.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1wyy.jpg",
                    Developer = "CD PROJEKT RED",
                    Publisher = "CD PROJEKT RED",
                    ReleaseDate = new DateTime(2015, 5, 19),
                    AgeRating = "18+",
                    Genres = new List<string> { "RPG", "Action", "Adventure" },
                    Platforms = new List<string> { "PC", "PlayStation", "Xbox", "Nintendo Switch" },
                    Tags = new List<string> { "Open World", "Fantasy", "Story Rich", "Medieval" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 7/8/10 64-bit | CPU: Intel Core i5-2500K | RAM: 6 GB | GPU: NVIDIA GTX 660 or AMD Radeon HD 7870",
                    RecommendedRequirements = "OS: Windows 7/8/10 64-bit | CPU: Intel Core i7-3770 | RAM: 8 GB | GPU: NVIDIA GTX 770 or AMD Radeon R9 290",
                    StockQuantity = 150,
                    IsAvailable = true,
                    IsFeatured = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "Elden Ring",
                    Description = "THE NEW FANTASY ACTION RPG. Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.",
                    ShortDescription = "Epic dark fantasy action RPG",
                    Price = 59.99m,
                    DiscountPrice = 47.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4jni.jpg",
                    Developer = "FromSoftware",
                    Publisher = "Bandai Namco Entertainment",
                    ReleaseDate = new DateTime(2022, 2, 25),
                    AgeRating = "16+",
                    Genres = new List<string> { "RPG", "Action", "Adventure" },
                    Platforms = new List<string> { "PC", "PlayStation", "Xbox" },
                    Tags = new List<string> { "Dark Fantasy", "Souls-like", "Open World", "Challenging" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 10 64-bit | CPU: Intel Core i5-8400 or AMD Ryzen 3 3300X | RAM: 12 GB | GPU: NVIDIA GTX 1060 or AMD Radeon RX 580",
                    RecommendedRequirements = "OS: Windows 10/11 64-bit | CPU: Intel Core i7-8700K or AMD Ryzen 5 3600X | RAM: 16 GB | GPU: NVIDIA GTX 1070 or AMD Radeon RX Vega 56",
                    StockQuantity = 120,
                    IsAvailable = true,
                    IsFeatured = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "Red Dead Redemption 2",
                    Description = "America, 1899. Arthur Morgan and the Van der Linde gang are outlaws on the run. With federal agents and the best bounty hunters in the nation massing on their heels, the gang must rob, steal and fight their way across the rugged heartland of America.",
                    ShortDescription = "Epic tale of life in America's unforgiving heartland",
                    Price = 59.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1q1f.jpg",
                    Developer = "Rockstar Games",
                    Publisher = "Rockstar Games",
                    ReleaseDate = new DateTime(2019, 11, 5),
                    AgeRating = "18+",
                    Genres = new List<string> { "Action", "Adventure", "Shooter" },
                    Platforms = new List<string> { "PC", "PlayStation", "Xbox" },
                    Tags = new List<string> { "Open World", "Western", "Story Rich", "Realistic" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 7 SP1 64-bit | CPU: Intel Core i5-2500K or AMD FX-6300 | RAM: 8 GB | GPU: NVIDIA GTX 770 or AMD Radeon R9 280",
                    RecommendedRequirements = "OS: Windows 10 64-bit | CPU: Intel Core i7-4770K or AMD Ryzen 5 1500X | RAM: 12 GB | GPU: NVIDIA GTX 1060 or AMD Radeon RX 480",
                    StockQuantity = 80,
                    IsAvailable = true,
                    IsFeatured = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "Baldur's Gate 3",
                    Description = "Gather your party and return to the Forgotten Realms in a tale of fellowship and betrayal, sacrifice and survival, and the lure of absolute power.",
                    ShortDescription = "Epic D&D adventure with rich storytelling",
                    Price = 59.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co4xr9.jpg",
                    Developer = "Larian Studios",
                    Publisher = "Larian Studios",
                    ReleaseDate = new DateTime(2023, 8, 3),
                    AgeRating = "18+",
                    Genres = new List<string> { "RPG", "Strategy", "Adventure" },
                    Platforms = new List<string> { "PC", "PlayStation", "Xbox" },
                    Tags = new List<string> { "Turn-Based", "D&D", "Story Rich", "Fantasy" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 10 64-bit | CPU: Intel i5-4690 or AMD FX 8350 | RAM: 8 GB | GPU: NVIDIA GTX 970 or AMD Radeon RX 480",
                    RecommendedRequirements = "OS: Windows 10 64-bit | CPU: Intel i7-8700K or AMD Ryzen 5 3600 | RAM: 16 GB | GPU: NVIDIA RTX 2060 or AMD Radeon RX 5700 XT",
                    StockQuantity = 95,
                    IsAvailable = true,
                    IsFeatured = false,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "Starfield",
                    Description = "In this next generation role-playing game set amongst the stars, create any character you want and explore with unparalleled freedom as you embark on an epic journey to answer humanity's greatest mystery.",
                    ShortDescription = "Epic space exploration RPG",
                    Price = 69.99m,
                    DiscountPrice = 55.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co6qe2.jpg",
                    Developer = "Bethesda Game Studios",
                    Publisher = "Bethesda Softworks",
                    ReleaseDate = new DateTime(2023, 9, 6),
                    AgeRating = "18+",
                    Genres = new List<string> { "RPG", "Action", "Adventure" },
                    Platforms = new List<string> { "PC", "Xbox" },
                    Tags = new List<string> { "Space", "Sci-Fi", "Open World", "Exploration" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 10 64-bit | CPU: AMD Ryzen 5 2600X or Intel Core i7-6800K | RAM: 16 GB | GPU: AMD Radeon RX 5700 or NVIDIA GTX 1070 Ti",
                    RecommendedRequirements = "OS: Windows 10/11 64-bit | CPU: AMD Ryzen 5 3600X or Intel i5-10600K | RAM: 16 GB | GPU: AMD Radeon RX 6800 XT or NVIDIA RTX 2080",
                    StockQuantity = 110,
                    IsAvailable = true,
                    IsFeatured = false,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "Hogwarts Legacy",
                    Description = "Hogwarts Legacy is an immersive, open-world action RPG. Now you can take control of the action and be at the center of your own adventure in the wizarding world.",
                    ShortDescription = "Open-world action RPG set in the Wizarding World",
                    Price = 59.99m,
                    DiscountPrice = 44.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co5vmg.jpg",
                    Developer = "Avalanche Software",
                    Publisher = "Warner Bros. Games",
                    ReleaseDate = new DateTime(2023, 2, 10),
                    AgeRating = "16+",
                    Genres = new List<string> { "RPG", "Action", "Adventure" },
                    Platforms = new List<string> { "PC", "PlayStation", "Xbox", "Nintendo Switch" },
                    Tags = new List<string> { "Magic", "Open World", "Fantasy", "Story Rich" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 10 64-bit | CPU: Intel Core i5-6600 or AMD Ryzen 5 1400 | RAM: 16 GB | GPU: NVIDIA GTX 960 or AMD Radeon RX 470",
                    RecommendedRequirements = "OS: Windows 10 64-bit | CPU: Intel Core i7-8700 or AMD Ryzen 5 3600 | RAM: 16 GB | GPU: NVIDIA GTX 1080 Ti or AMD Radeon RX 5700 XT",
                    StockQuantity = 130,
                    IsAvailable = true,
                    IsFeatured = false,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new Game
                {
                    Title = "God of War",
                    Description = "His vengeance against the Gods of Olympus years behind him, Kratos now lives as a man in the realm of Norse Gods and monsters. It is in this harsh, unforgiving world that he must fight to survive… and teach his son to do the same.",
                    ShortDescription = "Epic Norse mythology action-adventure",
                    Price = 49.99m,
                    DiscountPrice = 24.99m,
                    CoverImageUrl = "https://images.igdb.com/igdb/image/upload/t_cover_big/co1tmu.jpg",
                    Developer = "Santa Monica Studio",
                    Publisher = "Sony Interactive Entertainment",
                    ReleaseDate = new DateTime(2022, 1, 14),
                    AgeRating = "18+",
                    Genres = new List<string> { "Action", "Adventure" },
                    Platforms = new List<string> { "PC", "PlayStation" },
                    Tags = new List<string> { "Mythology", "Story Rich", "Action", "Adventure" },
                    ScreenshotUrls = new List<string>(),
                    MinimumRequirements = "OS: Windows 10 64-bit | CPU: Intel i5-2500K or AMD Ryzen 3 1200 | RAM: 8 GB | GPU: NVIDIA GTX 960 or AMD R9 290X",
                    RecommendedRequirements = "OS: Windows 10 64-bit | CPU: Intel i5-6600K or AMD Ryzen 5 2400G | RAM: 8 GB | GPU: NVIDIA GTX 1060 or AMD RX 570",
                    StockQuantity = 90,
                    IsAvailable = true,
                    IsFeatured = true,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            };

            logger.LogInformation($"Adding {games.Count} games to database...");
            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
            logger.LogInformation($"Successfully seeded {games.Count} games!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while seeding games");
            throw;
        }
    }
    }
}
