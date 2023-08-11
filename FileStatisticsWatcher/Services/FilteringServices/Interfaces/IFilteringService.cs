using FileStatisticsWatcher.Models.DTO.FilteringDTO;

namespace FileStatisticsWatcher.Services.FilteringServices.Interfaces
{
    public interface IFilteringService<T>
    {
        string GroupField { get; set; }
        List<SortFieldsSettings> SortSettings { get; set; }
        List<WhereSettings> WhereSettings { get; set; }

        IQueryable<T> GroupingList(IQueryable<T> list);
        IQueryable<T> SetFilters(IQueryable<T> list);
        IQueryable<T> SortinList(IQueryable<T> list);
        IQueryable<T> WhereList(IQueryable<T> list);
    }
}