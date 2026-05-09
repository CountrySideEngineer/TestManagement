namespace TestManagement.APP.Dto.TestLevel.Get
{
    public class GetTestLevelResponse
    {
        /// <summary>
        /// Unique identifier of the test level.
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Display name of the test level.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description or additional information for the test level.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
