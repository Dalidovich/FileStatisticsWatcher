using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;

namespace FileStatisticsWatcher.DAL.Repositories.Interfaces
{
    public interface IFileSettingsRepository
    {
        public Task<FileSettings> AddAsync(FileSettings entity);
        public IQueryable<FileSettings> Get();
        public Task<bool> SaveAsync();

        public IQueryable<DirectorySettings> GetDirictories();
    }
}
