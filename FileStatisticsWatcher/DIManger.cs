using FileStatisticsWatcher.DAL;
using FileStatisticsWatcher.DAL.Repositories;
using FileStatisticsWatcher.DAL.Repositories.Interfaces;
using FileStatisticsWatcher.Models;
using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;
using FileStatisticsWatcher.Services.BaseServices;
using FileStatisticsWatcher.Services.BaseServices.Interfaces;
using FileStatisticsWatcher.Services.FilteringServices;
using FileStatisticsWatcher.Services.FilteringServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace FileStatisticsWatcher
{
    public static class DIManger
    {
        public static void AddRepositores(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<IFileSettingsRepository, FileSettingsRepository>();
        }

        public static void AddFilteringServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddSingleton<IFilteringService<FileSettings>, FilteringFileService>();
            webApplicationBuilder.Services.AddSingleton<IFilteringService<DirectorySettings>, FilteringDirectoryService>();
        }
        public static void AddBaseServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<IFileSettingsService, FileSettingsService>();
            webApplicationBuilder.Services.AddScoped<IFileIOService, FileIOService>();
        }

        public static void AddOptions(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.Configure<ConnectionDatabaseSettings>(webApplicationBuilder.Configuration.GetSection("ConnectionDatabaseSettings"));
            webApplicationBuilder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<ConnectionDatabaseSettings>>().Value);
        }
        public static void AddDatabase(this WebApplicationBuilder webApplicationBuilder)
        {
            if (webApplicationBuilder.Configuration.GetSection("ConnectionDatabaseSettings").GetSection("isInMemory").Value == true.ToString())
            {
                webApplicationBuilder.Services.AddDbContext<AppDBContext>(opt =>
                {
                    opt.UseInMemoryDatabase(databaseName: "FluentValidationDemoDB");
                    opt.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
            }
            else
            {
                webApplicationBuilder.Services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(
                    webApplicationBuilder.Configuration.GetSection("ConnectionDatabaseSettings").GetSection("ConnectionString").Value));
            }
        }
    }
}