using FileStatisticsWatcher.DAL;
using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Services.BaseServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStatisticsWatcher.Services.BaseServices
{
    public class FileIOService : IFileIOService
    {
        private readonly AppDBContext context;

        public FileIOService(AppDBContext context)
        {
            this.context = context;
        }

        public FileInfo[] GetFiles(LoadSettings loadSettings)
        {
            var di = new DirectoryInfo(loadSettings.loadPath);
            var files = di.GetFiles().Where(f => !loadSettings.IgnorePaths.Any(str => f.FullName.Contains(str))).ToList();
            var filesInDirs = di.GetFiles().Where(f => !loadSettings.IgnorePaths.Any(str => f.FullName.Contains(str))).ToList();
            var subDirs = di.GetDirectories();

            bool last = false;

            while (!last)
            {
                filesInDirs = new List<FileInfo>();
                foreach (var d in subDirs)
                {
                    try
                    {
                        var tf = d.GetFiles().Where(f => !loadSettings.IgnorePaths.Any(str => f.FullName.Contains(str))).ToArray();

                        files.AddRange(tf);
                        filesInDirs.AddRange(tf);
                    }
                    catch (Exception) { }
                }

                var newSubDirs = new List<DirectoryInfo>();
                foreach (var dir in subDirs)
                {
                    try
                    {
                        newSubDirs.AddRange(dir.GetDirectories());
                    }
                    catch (Exception) { }
                }

                subDirs = newSubDirs.ToArray();

                if (filesInDirs.Count == 0 && subDirs.Length == 0)
                {
                    last = true;
                }
            }

            return files.ToArray();
        }

        public async Task resetDBAsync()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
    }
}
