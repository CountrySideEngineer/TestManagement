using System.ComponentModel.DataAnnotations;

namespace TestManagement.APP.Models.TestAnalysis
{
    public class Request
    {
        [Key]
        public int Key { get; set; }

        [Required]
        public string DirectoryPath { get; set; } = string.Empty;

        // Foreign key to StatusMaster
        public int StatusId { get;set; }
        public StatusMaster Status { get; set; } = new StatusMaster();

        // Foreign key to ResultMaster
        public int ResultId { get; set; }
        public ResultMaster Result { get; set; } = new ResultMaster();

        public DateTime RegisteredAt { get;set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
