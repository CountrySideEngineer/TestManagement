using System;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestManagement.APP.Dto.TestCase.Post;
using System.Net.Http.Json;

namespace TestManagement.APP.ApiClients.TestCase
{
    /// <summary>
    /// An API client that synchronizes test cases with the remote Test API.
    /// This implementation posts a collection of test case creation requests to the
    /// "Bulk/CreateIfNotExists" endpoint and returns the responses from the API.
    /// </summary>
    public class TestCaseSyncApiClient : ITestCaseSyncApiClient
    {
        // Logger instance provided by dependency injection for recording information and errors.
        private readonly ILogger<TestCaseSyncApiClient> _logger;

        // HttpClient used to call the remote Test API. This is created via IHttpClientFactory
        // to allow configuration (base address, timeouts, handlers) elsewhere in the app.
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructs a new instance of <see cref="TestCaseSyncApiClient"/>.
        /// </summary>
        /// <param name="logger">The logger to record diagnostic information.</param>
        /// <param name="httpClientFactory">Factory used to create an <see cref="HttpClient"/> configured for the Test API.</param>
        public TestCaseSyncApiClient(ILogger<TestCaseSyncApiClient> logger, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            // Create a named HttpClient instance. The name `TestApiClient` must match the registration.
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        /// <summary>
        /// Sends the provided collection of <see cref="PostTestCaseRequest"/> to the API endpoint
        /// "Bulk/CreateIfNotExists" and returns the responses as a collection of
        /// <see cref="PostTestCaseResponse"/>.
        /// </summary>
        /// <param name="requests">Collection of requests to post to the API.</param>
        /// <param name="ct">Cancellation token to cancel the request.</param>
        /// <returns>A collection of <see cref="PostTestCaseResponse"/> returned by the API.
        /// If the API returns a non-success status code, an empty collection is returned.</returns>
        public async Task<IEnumerable<PostTestCaseResponse>> SyncAsync(IEnumerable<PostTestCaseRequest> requests, CancellationToken ct = default)
        {
            if (requests is null) throw new ArgumentNullException(nameof(requests));

            try
            {
                // Post the requests as JSON to the Bulk/CreateIfNotExists endpoint.
                var response = await _httpClient
                    .PostAsJsonAsync("Bulk/CreateIfNotExists", requests, ct)
                    .ConfigureAwait(false);

                // If the API did not return success, log a warning and return an empty collection.
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("SyncAsync: API returned non-success status code {StatusCode}", response.StatusCode);
                    return Array.Empty<PostTestCaseResponse>();
                }
                else
                {
                    // Try to deserialize the JSON response body into the expected response type.
                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<PostTestCaseResponse>>(cancellationToken: ct).ConfigureAwait(false);
                    return result ?? Array.Empty<PostTestCaseResponse>();
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // Request was cancelled by the caller.
                _logger.LogInformation("SyncAsync was cancelled");
                throw;
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions and rethrow so callers can handle them.
                _logger.LogError(ex, "SyncAsync: Exception while calling Bulk/CreateIfNotExists");
                throw;
            }
        }
    }
}
