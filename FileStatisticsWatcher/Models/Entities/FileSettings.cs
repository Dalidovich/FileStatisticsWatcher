namespace FileStatisticsWatcher.Models.Entities
{
    public class FileSettings
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public int Depth { get; set; }
        public string Extension { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastAccessDateUTC { get; set; }
    }
}