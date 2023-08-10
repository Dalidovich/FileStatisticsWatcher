using FileStatisticsWatcher.DAL.Repositories.Interfaces;
using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;

namespace FileStatisticsWatcher.DAL.Repositories
{
    public class FileSettingsRepository : IFileSettingsRepository
    {
        private readonly AppDBContext _dbContext;

        public FileSettingsRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FileSettings> AddAsync(FileSettings entity)
        {
            var newEntity = await _dbContext.FileSettings.AddAsync(entity);

            return newEntity.Entity;
        }

        public IQueryable<FileSettings> Get()
        {

            return _dbContext.FileSettings;
        }

        public IQueryable<DirectorySettings> GetDirictories()
        {
            var tempTable1 =
                from fs in _dbContext.FileSettings
                group fs by fs.Path into g
                select new
                {
                    Path = g.Key,
                    TotalSize = g.Sum(fs => fs.Size)
                };

            var result =
                from fs in _dbContext.FileSettings
                where tempTable1.Select(tt => tt.Path).Contains(fs.Path)
                group fs by fs.Path into g
                orderby g.Key, g.Sum(fs => fs.Size) descending
                select new DirectorySettings()
                {
                    Name = g.Key.Substring(g.Key.LastIndexOf('\\') + 1),
                    Path = g.Key,
                    CountFiles = g.Count(),
                    FilesSize = g.Sum(fs => fs.Size),
                    DirictorySize = _dbContext.FileSettings.Where(fs => fs.Path.StartsWith(g.Key)).Sum(fs => fs.Size)
                };

            return result;
        }

        public async Task<bool> SaveAsync()
        {
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
