namespace TestManagement.API.Features.Tester.Create;

/// <summary>
/// Request DTO used to create a new tester.
/// </summary>
public class CreateTesterRequest
{
    /// <summary>
    /// Tester name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the tester.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}