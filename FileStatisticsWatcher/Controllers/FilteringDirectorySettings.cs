using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.DTO.FilteringDTO;
using FileStatisticsWatcher.Services.BaseServices;
using FileStatisticsWatcher.Services.FilteringServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileStatisticsWatcher.Controllers
{
    public class FilteringDirectorySettings : Controller
    {
        private readonly IFilteringService<DirectorySettings> _filteringService;

        public FilteringDirectorySettings(IFilteringService<DirectorySettings> filteringService)
        {
            _filteringService = filteringService;
        }

        public IActionResult UpdateSortingSettings(SortFieldsSettings sortFieldsSettings, bool add)
        {
            if (!ObjectFieldManager.GetFields<DirectorySettings>().Contains(sortFieldsSettings.FieldName))
            {

                return RedirectToAction("FilteringForms");
            }
            if (add)
            {
                if (_filteringService.SortSettings.FindIndex(x => x.FieldName == sortFieldsSettings.FieldName) == -1)
                {
                    _filteringService.SortSettings.Add(sortFieldsSettings);
                }
            }
            else
            {
                _filteringService.SortSettings.RemoveAt(_filteringService.SortSettings.FindIndex(x => x.FieldName == sortFieldsSettings.FieldName));
            }

            return RedirectToAction("FilteringForms");
        }

        public IActionResult UpdateGroupSettings(string fieldName, bool add)
        {
            if (!ObjectFieldManager.GetFields<DirectorySettings>().Contains(fieldName))
            {

                return RedirectToAction("FilteringForms");
            }
            if (add)
            {
                _filteringService.GroupField = fieldName;
            }
            else
            {
                _filteringService.GroupField = string.Empty;
            }

            return RedirectToAction("FilteringForms");
        }

        public IActionResult UpdateWhereSettings(WhereSettings whereSettings, bool add)
        {
            if (!ObjectFieldManager.GetFields<DirectorySettings>().Contains(whereSettings.FieldName))
            {

                return RedirectToAction("FilteringForms");
            }
            if (add)
            {
                _filteringService.WhereSettings.Add(whereSettings);
            }
            else
            {
                _filteringService.WhereSettings.RemoveAt(_filteringService.WhereSettings.FindIndex(x =>
                {
                    return x.FieldName == whereSettings.FieldName &&
                        x.NotFlag == whereSettings.NotFlag &&
                        x.TargetValue == whereSettings.TargetValue &&
                        x.State == whereSettings.State;
                }));
            }

            return RedirectToAction("FilteringForms");
        }

        public IActionResult ResetFilters()
        {
            _filteringService.GroupField = string.Empty;
            _filteringService.WhereSettings.Clear();
            _filteringService.SortSettings.Clear();

            return RedirectToAction("FilteringForms");
        }

        public IActionResult FilteringForms()
        {
            return View();
        }
    }
}
