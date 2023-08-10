namespace FileStatisticsWatcher.Models.DTO
{
	public class LoadSettings
	{
		public string loadPath { get; set; }
		public List<string> IgnorePaths { get; set; } = new List<string>();
	}
}
