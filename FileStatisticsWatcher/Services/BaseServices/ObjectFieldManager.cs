namespace FileStatisticsWatcher.Services.BaseServices
{
    public static class ObjectFieldManager
    {
        public static string[] GetFields<T>()
        {
            var fields = new List<string>();

            var type = typeof(T);
            foreach (var field in type.GetProperties())
            {
                fields.Add(field.Name);
            }

            return fields.ToArray();
        }
    }
}