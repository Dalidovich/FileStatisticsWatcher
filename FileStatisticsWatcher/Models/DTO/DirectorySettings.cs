namespace FileStatisticsWatcher.Models.DTO
{
    public class DirectorySettings
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long FilesSize { get; set; }
        public long CountFiles { get; set; }
        public long DirictorySize { get; set; }
    }
}