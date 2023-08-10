using FileStatisticsWatcher.DAL.Repositories.Interfaces;
using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;
using FileStatisticsWatcher.Services.Filtering;
using FileStatisticsWatcher.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStatisticsWatcher.Services
{
    public class FileSettingsService : IFileSettingsService
    {
        private readonly IFileSettingsRepository _repository;
        private readonly IFileIOService _fileIOService;
        private readonly IFilteringService<FileSettings> _filteringService;

        public FileSettingsService(IFileSettingsRepository repository, IFileIOService fileIOService, IFilteringService<FileSettings> filteringService)
        {
            _repository = repository;
            _fileIOService = fileIOService;
            _filteringService = filteringService;
        }

        public async Task<DirectorySettings[]> GetStatisticsForDirs()
        {
            var list = await _repository.GetDirictories().AsNoTracking().ToArrayAsync();

            return list;
        }

        public async Task<FileSettings[]> GetFileSettings()
        {
            var listQ = _repository.Get();

            var list= await _filteringService.SetFilters(listQ).AsNoTracking().ToArrayAsync();

            return list;
        }

        public async Task<bool> ReloadFiles(LoadSettings loadSettings)
        {
			loadSettings.loadPath = loadSettings.loadPath ?? Directory.GetCurrentDirectory();
            await _fileIOService.resetDBAsync();
            var filesInfo = _fileIOService.GetFiles(loadSettings);
            foreach (var file in filesInfo) 
            {
                await _repository.AddAsync(new FileSettings()
                {
                    Name = file.Name,
                    Path = file.DirectoryName,
                    Size = file.Length,
                    Extension = file.Extension,
                    Depth = file.FullName.Count(x => x == '\\') - 1,
                    CreateDateUTC = file.CreationTimeUtc,
                    LastAccessDateUTC = file.Directory.LastAccessTimeUtc,
                    LastWriteDateUTC = file.Directory.LastAccessTimeUtc,
                });
            }

            return await _repository.SaveAsync();
        }
    }
}
