using System.ComponentModel.DataAnnotations;

namespace TestManagement.APP.Models.TestAnalysis
{
    public class Request
    {
        /// <summary>
        /// Primary identifier for the request.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Filesystem directory path that this request targets.
        /// </summary>
        [Required]
        public string DirectoryPath { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the associated test level.
        /// </summary>
        [Required]
        public int TestLevelId { get; set; } = 0;

        /// <summary>
        /// Foreign key to the <see cref="StatusMaster"/> that represents the request status.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="StatusMaster"/> entity.
        /// </summary>
        public StatusMaster Status { get; set; } = new StatusMaster();

        /// <summary>
        /// Foreign key to the <see cref="ResultMaster"/> that represents the request result.
        /// </summary>
        public int ResultId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ResultMaster"/> entity.
        /// </summary>
        public ResultMaster Result { get; set; } = new ResultMaster();

        /// <summary>
        /// UTC timestamp when the request was registered.
        /// </summary>
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the request was last updated.
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
