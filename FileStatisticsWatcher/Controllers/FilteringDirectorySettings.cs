using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.DTO.FilteringDTO;
using FileStatisticsWatcher.Services.FilteringServices;
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
            if (add)
            {
                if (_filteringService.SortSettings.FindIndex(x => x.FieldName == sortFieldsSettings.FieldName) == -1)
                {
                    _filteringService.SortSettings.Add(sortFieldsSettings);
                }
            }
            else
            {
                var deleteIndex = _filteringService.SortSettings.FindIndex(x => x.FieldName == sortFieldsSettings.FieldName);
                if (deleteIndex != -1)
                {
                    _filteringService.SortSettings.RemoveAt(deleteIndex);
                }
            }

            return RedirectToAction("FilteringForms");
        }

        public IActionResult UpdateGroupSettings(string fieldName, bool add)
        {
            if (add)
            {
                _filteringService.GroupField = fieldName;
            }
            else
            {
                _filteringService.GroupField = fieldName == _filteringService.GroupField ? string.Empty : _filteringService.GroupField;
            }

            return RedirectToAction("FilteringForms");
        }

        public IActionResult UpdateWhereSettings(WhereSettings whereSettings, bool add)
        {
            if (add)
            {
                _filteringService.WhereSettings.Add(whereSettings);
            }
            else
            {
                var deleteIndex = _filteringService.WhereSettings.FindIndex(x =>
                {
                    return x.FieldName == whereSettings.FieldName &&
                        x.NotFlag == whereSettings.NotFlag &&
                        x.TargetValue == whereSettings.TargetValue &&
                        x.State == whereSettings.State;
                });
                if (deleteIndex != -1)
                {
                    _filteringService.WhereSettings.RemoveAt(deleteIndex);
                }
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
