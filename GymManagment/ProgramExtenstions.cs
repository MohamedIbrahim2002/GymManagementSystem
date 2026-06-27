using GymManagment.DAL.Data.DataSeeding;
using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.PL
{
    public static class ProgramExtenstions
    {
        public  static async Task MigrateAndSeedDarabaseAsync(this WebApplication app)

        {

            using var scope = app.Services.CreateScope();

            var dbContext=scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            var penddingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (penddingMigrations.Any())
            {
                logger.LogInformation($"Applying {penddingMigrations.Count()}");
                await dbContext.Database.MigrateAsync();
            }

            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath , logger);


             await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager, logger);

        }
    }
}
