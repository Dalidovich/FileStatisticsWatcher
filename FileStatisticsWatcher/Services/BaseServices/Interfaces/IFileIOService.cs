using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;

namespace FileStatisticsWatcher.Services.BaseServices.Interfaces
{
    public interface IFileIOService
    {
        Task resetDBAsync();
        FileInfo[] GetFiles(LoadSettings loadSettings);
    }
}
