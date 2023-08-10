using FileStatisticsWatcher.DAL;
using FileStatisticsWatcher.DAL.Repositories;
using FileStatisticsWatcher.DAL.Repositories.Interfaces;
using FileStatisticsWatcher.Models.Entities;
using FileStatisticsWatcher.Services;
using FileStatisticsWatcher.Services.Filtering;
using FileStatisticsWatcher.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStatisticsWatcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IFilteringService<FileSettings> ,FilteringFileService >();

            builder.Services.AddScoped<IFileSettingsService,FileSettingsService>();
            builder.Services.AddScoped<IFileIOService, FileIOService>();
            builder.Services.AddScoped<IFileSettingsRepository, FileSettingsRepository>();

            builder.Services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(
                builder.Configuration.GetConnectionString("NpgConnectionString")));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/FileSettings/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=FileSettings}/{action=Index}");

            app.Run();
        }
    }
}