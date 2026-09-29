using TestManagement.API.Features.Tester.Create;
using TestManagement.API.Features.Tester.Get;

namespace TestManagement.API.Services;

/// <summary>
/// Service interface for managing tester entities and related operations.
/// Defines methods for retrieving, creating, and managing tester data.
/// </summary>
public interface ITesterService
{
    /// <summary>
    /// Retrieves all testers asynchronously.
    /// </summary>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A collection of <see cref="GetTesterResponse"/> objects representing all testers.</returns>
    Task<ICollection<GetTesterResponse>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Retrieves a specific tester by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the tester to retrieve.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="GetTesterResponse"/> object representing the tester.</returns>
    Task<GetTesterResponse> GetByIdAsync(long id, CancellationToken ct);

    /// <summary>
    /// Creates a new tester asynchronously.
    /// </summary>
    /// <param name="request">The <see cref="CreateTesterRequest"/> containing the tester's information.</param>
    /// <param name="ct">Cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="CreateTesterResponse"/> containing the created tester's details including the generated identifier.</returns>
    Task<CreateTesterResponse> CreateAsync(CreateTesterRequest request, CancellationToken ct);
}
