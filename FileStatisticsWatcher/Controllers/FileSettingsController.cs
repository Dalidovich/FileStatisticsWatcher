using FileStatisticsWatcher.Models;
using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileStatisticsWatcher.Controllers
{
    public class FileSettingsController : Controller
    {
        private readonly IFileSettingsService _fileSettingsService;

        public FileSettingsController(IFileSettingsService fileSettingsService)
        {
            _fileSettingsService = fileSettingsService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("AllFiles");
        }

        public async Task<IActionResult> AllFiles()
        {
            var list = await _fileSettingsService.GetFileSettings();

            return View("Index", list);
        }

		public IActionResult LoadFiles()
		{

            return View();
		}

        [HttpPost]
		public async Task<IActionResult> LoadFiles(LoadSettings loadSettings, string ignorePath)
        {
            ignorePath = ignorePath ?? string.Empty;
            if (ignorePath.Trim(',').IndexOf(",") != -1)
			{
				loadSettings.IgnorePaths = ignorePath.Trim(',').Split(',').ToList();
			}
            else if (ignorePath != null && ignorePath != "")
            {
                loadSettings.IgnorePaths.Add(ignorePath.Trim(','));
            }
            await _fileSettingsService.ReloadFiles(loadSettings);

            return RedirectToAction("AllFiles");
        }

        public async Task<IActionResult> StatisticsForDirs()
        {
            var list = await _fileSettingsService.GetStatisticsForDirs();

            return View("DirectoriesSettings", list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}