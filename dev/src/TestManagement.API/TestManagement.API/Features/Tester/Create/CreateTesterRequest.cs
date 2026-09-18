namespace TestManagement.API.Features.Tester.Create;

/// <summary>
/// Request DTO used to create a new tester.
/// </summary>
public class CreateTesterRequest
{
    /// <summary>
    /// Gets or sets the tester's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tester's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}