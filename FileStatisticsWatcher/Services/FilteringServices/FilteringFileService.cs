using FileStatisticsWatcher.Models.DTO.FilteringDTO;
using FileStatisticsWatcher.Models.Entities;
using FileStatisticsWatcher.Services.FilteringServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FileStatisticsWatcher.Services.FilteringServices
{
    public class FilteringFileService : IFilteringService<FileSettings>
    {
        public FilteringFileService()
        { }

        public List<SortFieldsSettings> SortSettings { get; set; } = new() { };
        public List<WhereSettings> WhereSettings { get; set; } = new() { };
        public string GroupField { get; set; } = string.Empty;

        public IQueryable<FileSettings> SetFilters(IQueryable<FileSettings> list)
        {
            list = WhereList(list);
            list = GroupingList(list);
            list = SortinList(list);
            return list;
        }

        public static readonly string[] whereAvailableParams = { "depth", "size", "path", "name", "create", "createdate", "writedate", "accessdate" };
        public IQueryable<FileSettings> WhereList(IQueryable<FileSettings> list)
        {
            var fitsList = list.Where(x => x.Id != null);
            foreach (var item in WhereSettings)
            {
                switch (item.FieldName.ToLower())
                {
                    case "depth":
                        if (int.TryParse(item.TargetValue, out var result1))
                        {
                            var filter = item.CreateFilter<FileSettings, int>(o => o.Depth, result1, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    case "size":
                        if (long.TryParse(item.TargetValue, out var result2))
                        {
                            var filter = item.CreateFilter<FileSettings, long>(o => o.Size, result2, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    case "path":
                        if ((int)item.State < 1)
                        {
                            var filter = item.CreateFilter<FileSettings, string>(o => o.Path, item.TargetValue, item.State);
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
                    case "name":
                        if ((int)item.State < 1)
                        {
                            var filter = item.CreateFilter<FileSettings, string>(o => o.Name, item.TargetValue, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        else
                        {
                            switch (item.State)
                            {
                                case WhereState.contains:
                                    fitsList = fitsList.Where(x => x.Name.Contains(item.TargetValue));
                                    break;
                                case WhereState.startWith:
                                    fitsList = fitsList.Where(x => x.Name.StartsWith(item.TargetValue));
                                    break;
                                case WhereState.endWith:
                                    fitsList = fitsList.Where(x => x.Name.EndsWith(item.TargetValue));
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "createdate":
                        if (DateTime.TryParse(item.TargetValue, out var cd))
                        {
                            var filter = item.CreateFilter<FileSettings, DateTime>(o => o.CreateDateUTC, cd, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    case "writedate":
                        if (DateTime.TryParse(item.TargetValue, out var wd))
                        {
                            var filter = item.CreateFilter<FileSettings, DateTime>(o => o.LastWriteDateUTC, wd, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    case "accessdate":
                        if (DateTime.TryParse(item.TargetValue, out var ad))
                        {
                            var filter = item.CreateFilter<FileSettings, DateTime>(o => o.LastAccessDateUTC, ad, item.State);
                            fitsList = fitsList.Where(filter);
                        }
                        break;
                    default:
                        return list;
                }
            }

            return fitsList;
        }


        public static readonly string[] sortAvailableParams = { "depth", "size", "path", "name", "create", "createdate", "writedate", "accessdate" };
        public IQueryable<FileSettings> SortinList(IQueryable<FileSettings> list)
        {
            IOrderedQueryable<FileSettings> orderingList = list.OrderBy(p => 0);

            Expression<Func<FileSettings, int>> pathExp = (y) => y.Path.Length;
            Expression<Func<FileSettings, int>> nameExp = (y) => y.Name.Length;
            Expression<Func<FileSettings, long>> sizeExp = (y) => y.Size;
            Expression<Func<FileSettings, int>> depthExp = (y) => y.Depth;
            Expression<Func<FileSettings, DateTime>> createExp = (y) => y.CreateDateUTC;
            Expression<Func<FileSettings, DateTime>> writeExp = (y) => y.LastWriteDateUTC;
            Expression<Func<FileSettings, DateTime>> accessExp = (y) => y.LastAccessDateUTC;

            foreach (var item in SortSettings)
            {
                switch (item.FieldName.ToLower())
                {
                    case "path":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(pathExp) :
                            orderingList.ThenByDescending(pathExp);
                        break;
                    case "name":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(nameExp) :
                            orderingList.ThenByDescending(nameExp);
                        break;
                    case "size":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(sizeExp) :
                            orderingList.ThenByDescending(sizeExp);
                        break;
                    case "depth":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(depthExp) :
                            orderingList.ThenByDescending(depthExp);
                        break;
                    case "createdate":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(createExp) :
                            orderingList.ThenByDescending(createExp);
                        break;
                    case "writedate":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(writeExp) :
                            orderingList.ThenByDescending(writeExp);
                        break;
                    case "accessdate":
                        orderingList = item.FieldDirection == SortFieldsSettings.ASC ?
                            orderingList.ThenBy(accessExp) :
                            orderingList.ThenByDescending(accessExp);
                        break;
                    default:
                        return list;
                }
            }

            return orderingList ?? list;
        }

        public static readonly string[] groupAvailableParams = { "depth", "size", "path", "name", "extension"};
        public IQueryable<FileSettings> GroupingList(IQueryable<FileSettings> list)
        {
            switch (GroupField.ToLower())
            {
                case "depth":
                    list =
                        from fs in list
                        group fs by fs.Depth into g
                        select new FileSettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = $"count={g.Count()}",
                            Depth = g.Key,
                            Size = g.Sum(x => x.Size),
                            Extension = string.Join(",", g.Select(k => k.Extension).Select(x => $"{x}(" +
                                $"{g.GroupBy(e => e.Extension).Select(y => new { exp = y.Key, count = y.Count() }).Single(o => o.exp == x).count})").ToHashSet()),
                        };
                    break;
                case "path":
                    list =
                        from fs in list
                        group fs by fs.Path into g
                        select new FileSettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = g.Key,
                            Depth = g.Count(),
                            Size = g.Sum(x => x.Size),
                            Extension = string.Join(",", g.Select(k => k.Extension).Select(x => $"{x}(" +
                                $"{g.GroupBy(e => e.Extension).Select(y => new { exp = y.Key, count = y.Count() }).Single(o => o.exp == x).count})").ToHashSet()),
                        };
                    break;
                case "name":
                    list =
                        from fs in list
                        group fs by fs.Name into g
                        select new FileSettings()
                        {
                            Name = g.Key,
                            Path = $"count={g.Count()}",
                            Depth = g.Count(),
                            Size = g.Sum(x => x.Size),
                            Extension = string.Join(",", g.Select(k => k.Extension).Select(x => $"{x}(" +
                                $"{g.GroupBy(e => e.Extension).Select(y => new { exp = y.Key, count = y.Count() }).Single(o => o.exp == x).count})").ToHashSet()),
                        };
                    break;
                case "size":
                    list =
                        from fs in list
                        group fs by fs.Size into g
                        select new FileSettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = $"count={g.Count()}",
                            Depth = g.Count(),
                            Size = g.Key,
                            Extension = string.Join(",", g.Select(k => k.Extension).Select(x => $"{x}(" +
                                $"{g.GroupBy(e => e.Extension).Select(y => new { exp = y.Key, count = y.Count() }).Single(o => o.exp == x).count})").ToHashSet()),
                        };
                    break;
                case "extension":
                    list =
                        from fs in list
                        group fs by fs.Name into g
                        select new FileSettings()
                        {
                            Name = $"count={g.Count()}",
                            Path = $"count={g.Count()}",
                            Depth = g.Count(),
                            Size = g.Sum(x => x.Size),
                            Extension = g.Key,
                        };
                    break;
            }
            return list;
        }
    }
}
