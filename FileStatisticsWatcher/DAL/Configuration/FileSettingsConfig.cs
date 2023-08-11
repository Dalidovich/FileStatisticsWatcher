using FileStatisticsWatcher.DAL.Configuration.DataType;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FileStatisticsWatcher.Models.Entities;

namespace FileStatisticsWatcher.DAL.Configuration
{
    public class FileSettingsConfig : IEntityTypeConfiguration<FileSettings>
    {
        public const string Table_name = "file_settings";

        public void Configure(EntityTypeBuilder<FileSettings> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });
            builder.HasIndex(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnType(EntityDataTypes.Guid)
                .HasColumnName("pk_file_settings_id");

            builder.Property(e => e.Name)
                .HasColumnType(EntityDataTypes.Character_varying)
                .HasColumnName("name");

            builder.Property(e => e.Path)
                .HasColumnType(EntityDataTypes.Character_varying)
                .HasColumnName("path");

            builder.Property(e => e.Size)
                .HasColumnType(EntityDataTypes.Bigint)
                .HasColumnName("size");

            builder.Property(e => e.Depth)
                .HasColumnType(EntityDataTypes.Smallint)
                .HasColumnName("depth");

            builder.Property(e => e.Extension)
                .HasColumnType(EntityDataTypes.Character_varying)
                .HasColumnName("extension");

            builder.Property(e => e.CreateDateUTC)
                .HasColumnName("create_date");

            builder.Property(e => e.LastAccessDateUTC)
                .HasColumnName("last_access_date");
        }
    }
}
