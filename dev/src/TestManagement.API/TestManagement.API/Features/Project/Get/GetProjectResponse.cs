namespace TestManagement.API.Features.Project.Get
{
    public class GetProjectResponse
    {
        /// <summary>
        /// The unique identifier of the project.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The display name of the project.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A short description of the project.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        //TODO: Add collection of TestSuite models to the response.

        //TODO: Add collection of TestExecution models to the response.

        //TODO: Add collection of Tester models to the reponse.
    }
}
