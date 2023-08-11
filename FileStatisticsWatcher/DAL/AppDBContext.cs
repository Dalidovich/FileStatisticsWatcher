using FileStatisticsWatcher.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FileStatisticsWatcher.DAL
{
    public class AppDBContext : DbContext
    {
        public DbSet<FileSettings> FileSettings { get; set; }

        public AppDBContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
