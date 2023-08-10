﻿using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.Entities;

namespace FileStatisticsWatcher.Services.Interfaces
{
    public interface IFileSettingsService
    {
        Task<bool> ReloadFiles(LoadSettings loadSettings);
        Task<FileSettings[]> GetFileSettings();
        Task<DirectorySettings[]> GetStatisticsForDirs();
    }
}
