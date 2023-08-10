namespace FileStatisticsWatcher.Models.DTO.FilteringDTO
{
    public static class WhereEnumHelper
    {
        public static string[] GetEnumsList()
        {
            var type = typeof(WhereState);
            var arr = new string[type.GetEnumNames().Length];

            var names = type.GetEnumNames();

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = names[i];
            }

            return arr;
        }
    }
}
