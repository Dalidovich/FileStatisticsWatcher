﻿using FileStatisticsWatcher.DAL;
using Microsoft.EntityFrameworkCore;

namespace FileStatisticsWatcher.BackgroundHostedService
{
    public class CheckDBHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private AppDBContext _appDBContext;

        public CheckDBHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            if (await _appDBContext.Database.EnsureCreatedAsync())
            {
                await _appDBContext.Database.MigrateAsync();
            }

            return;
        }
    }
}
