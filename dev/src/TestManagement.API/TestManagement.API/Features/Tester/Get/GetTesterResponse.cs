namespace TestManagement.API.Features.Tester.Get;

/// <summary>
/// Response DTO for retrieving tester information.
/// Used to return tester data from read operations.
/// </summary>
public class GetTesterResponse
{
    /// <summary>
    /// Gets or sets the identifier of the tester.
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
