namespace TestManagement.API.Features.Tester.Create;

/// <summary>
/// Response DTO returned after creating a new tester.
/// Contains the details of the successfully created tester entity.
/// </summary>
public class CreateTesterResponse
{
    /// <summary>
    /// Gets or sets the identifier of the created tester.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the tester's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tester's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}