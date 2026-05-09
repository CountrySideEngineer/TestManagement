namespace TestManagement.APP.Models
{
    /// <summary>
    /// Data Transfer Object that represents a test level.
    /// </summary>
    public class TestLevelDto
    {
        /// <summary>
        /// Unique identifier of the test level.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the test level.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description or notes for the test level.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
