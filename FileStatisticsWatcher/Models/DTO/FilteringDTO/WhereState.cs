namespace FileStatisticsWatcher.Models.DTO.FilteringDTO
{
    public enum WhereState
    {
        eq = 0,
        contains = 2,
        startWith = 3,
        endWith = 4,
        more = 1,
        less = -1,
    }
}
