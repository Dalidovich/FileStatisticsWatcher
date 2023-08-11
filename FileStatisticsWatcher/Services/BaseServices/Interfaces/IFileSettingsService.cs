using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;

namespace FileStatisticsWatcher.Services.BaseServices.Interfaces
{
    public interface IFileSettingsService
    {
        Task<bool> ReloadFiles(LoadSettings loadSettings);
        Task<FileSettings[]> GetFileSettings(int page);
        Task<DirectorySettings[]> GetStatisticsForDirs(int page);
    }
}
