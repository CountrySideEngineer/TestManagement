namespace TestManagement.API.Features.Tester.Create;

/// <summary>
/// Response DTO returned after creating a new tester through the API.
/// </summary>
public class TesterCreateResponse
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