namespace FileStatisticsWatcher.Models.DTO.FilteringDTO
{
    public class SortFieldsSettings
    {
        public static readonly bool ASC = false;
        public string FieldName { get; set; }
        public bool FieldDirection { get; set; }
    }
}
