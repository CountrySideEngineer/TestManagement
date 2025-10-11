namespace TestManagement.Models
{
    public class XmlImportHistory
    {
        public int ImportId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime ImportedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
