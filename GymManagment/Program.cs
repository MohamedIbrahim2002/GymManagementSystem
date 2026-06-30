using AutoMapper;
using GymManagment.BLL;
using GymManagment.BLL.Services.Attachment;
using GymManagment.BLL.Services.Classes;
using GymManagment.BLL.Services.Interfaces;
using GymManagment.DAL;
using GymManagment.DAL.Data.DataSeeding;
using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
namespace GymManagment
{
    public class Program
    {
        public static async Task Main(string[] args)

        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Register the PlanRepository as the implementation for IPlanRepository

            builder.Services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
            builder.Services.AddScoped<IMemberServices,MemberService>();
            builder.Services.AddScoped<IPlanServices,PlanService>();
            builder.Services.AddScoped<ITrainerServices,TrainerService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IMembershipRepository ,MembershipRepository>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService,BookingService>();
            builder.Services.AddScoped<IAnalaticServices, AnalaticService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAttachmenServices , AttachmenServices>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                config.Lockout.MaxFailedAccessAttempts = 5;

            })
                .AddEntityFrameworkStores<GymDbContext>();
             

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            var app = builder.Build();
            
            // SeedData

            await app.MigrateAndSeedDarabaseAsync();
           
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            

            app.Run();
        }
    }
}
