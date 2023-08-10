namespace FileStatisticsWatcher.Services
{
    public static class HelperService
    {
        public static string FormatByteSize(this long bytes)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            if (bytes == 0)
            {

                return "0 Bytes";
            }
            int i = (int)Math.Floor(Math.Log(bytes) / Math.Log(1024));
            double formattedSize = Math.Round(bytes / Math.Pow(1024, i), 2);

            return $"{formattedSize} {sizes[i]}";
        }
    }
}
