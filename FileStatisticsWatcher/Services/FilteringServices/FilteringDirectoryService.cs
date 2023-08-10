using FileStatisticsWatcher.Models.DTO;
using FileStatisticsWatcher.Models.DTO.FilteringDTO;
using FileStatisticsWatcher.Services.FilteringServices.Interfaces;
using System.Linq.Expressions;

namespace FileStatisticsWatcher.Services.FilteringServices
{
    public class FilteringDirectoryService : IFilteringService<DirectorySettings>
    {
        public List<SortFieldsSettings> SortSettings { get; set; } = new() { };
        public List<WhereSettings> WhereSettings { get; set; } = new() { };
        public string GroupField { get; set; } = string.Empty;

        public FilteringDirectoryService()
        {
        }

        public IQueryable<DirectorySettings> GroupingList(IQueryable<DirectorySettings> list)
        {
            switch (GroupField.ToLower())
            {
                case "countfiles":
                    list =
                        from fs in list
                        group fs by fs.CountFiles into g
                        select new DirectorySettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = $"__",
                            CountFiles = g.Key,
                            FilesSize = g.Sum(x => x.FilesSize),
                            DirictorySize = g.Sum(x => x.DirictorySize),
                        };
                    break;
                case "dirictorysize":
                    list =
                        from fs in list
                        group fs by fs.DirictorySize into g
                        select new DirectorySettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = $"__",
                            CountFiles = g.Sum(x => x.CountFiles),
                            FilesSize = g.Sum(x => x.FilesSize),
                            DirictorySize = g.Key,
                        };
                    break;
                case "filessize":
                    list =
                        from fs in list
                        group fs by fs.FilesSize into g
                        select new DirectorySettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = $"__",
                            CountFiles = g.Sum(x => x.CountFiles),
                            FilesSize = g.Key,
                            DirictorySize = g.Sum(x => x.DirictorySize),
                        };
                    break;
            }
            return list;
        }

        public IQueryable<DirectorySettings> SetFilters(IQueryable<DirectorySettings> list)
        {
            list = WhereList(list);
            list = GroupingList(list);
            list = SortinList(list);
            return list;
        }

        public IQueryable<DirectorySettings> SortinList(IQueryable<DirectorySettings> list)
        {
            IOrderedQueryable<DirectorySettings> orderingList = list.OrderBy(p => 0);

            Expression<Func<DirectorySettings, int>> pathExp = (y) => y.Path.Length;
            Expression<Func<DirectorySettings, long>> countFilesExp = (y) => y.CountFiles;
            Expression<Func<DirectorySettings, long>> filesSizeExp = (y) => y.FilesSize;
            Expression<Func<DirectorySettings, long>> dirictorySizeExp = (y) => y.DirictorySize;

            foreach (var item in SortSettings)
            {
                switch (item.FieldName.ToLower())
                {
                    case "path":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(pathExp) :
                            orderingList.ThenByDescending(pathExp);
                        break;
                    case "countfiles":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(countFilesExp) :
                            orderingList.ThenByDescending(countFilesExp);
                        break;
                    case "filessize":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(filesSizeExp) :
                            orderingList.ThenByDescending(filesSizeExp);
                        break;
                    case "dirictorysize":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(dirictorySizeExp) :
                            orderingList.ThenByDescending(dirictorySizeExp);
                        break;
                    default:
                        return list;
                }
            }

            return orderingList ?? list;
        }

        public IQueryable<DirectorySettings> WhereList(IQueryable<DirectorySettings> list)
        {
            var fitsList = list.Where(x => x.Path != null);
            foreach (var item in WhereSettings)
            {
                switch (item.FieldName.ToLower())
                {
                    case "filessize":
                        if (long.TryParse(item.TargetValue, out var result1))
                        {
                            var filter = item.CreateFilter<DirectorySettings, long>(o => o.FilesSize, result1, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    case "countfiles":
                        if (long.TryParse(item.TargetValue, out var result2))
                        {
                            var filter = item.CreateFilter<DirectorySettings, long>(o => o.CountFiles, result2, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    case "path":
                        if ((int)item.State < 1)
                        {
                            var filter = item.CreateFilter<DirectorySettings, string>(o => o.Path, item.TargetValue, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        else
                        {
                            switch (item.State)
                            {
                                case WhereState.contains:
                                    fitsList = fitsList.Where(x => x.Path.Contains(item.TargetValue));
                                    break;
                                case WhereState.startWith:
                                    fitsList = fitsList.Where(x => x.Path.StartsWith(item.TargetValue));
                                    break;
                                case WhereState.endWith:
                                    fitsList = fitsList.Where(x => x.Path.EndsWith(item.TargetValue));
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        return list;
                }
            }

            return fitsList;
        }
    }
}
