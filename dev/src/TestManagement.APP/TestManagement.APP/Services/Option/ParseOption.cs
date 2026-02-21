namespace TestManagement.APP.Services.Option
{
    /// <summary>
    /// Options used when parsing a file.
    /// </summary>
    public class ParseOption
    {
        /// <summary>
        /// Identifier of the test level. Optional (when null the default level is used).
        /// </summary>
        public int? TestLevelId { get; set; } 

        /// <summary>
        /// Revision identifier. Specifies the version of the upload or the item being parsed.
        /// </summary>
        public int RevisionId { get; set; }
    }
}
