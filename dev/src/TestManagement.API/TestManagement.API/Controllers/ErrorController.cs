using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TestManagement.API.Controllers
{
    [ApiController]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        public IActionResult HandleError()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = feature?.Error;

            var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;

            // ProblemDetails を生成して例外種別に応じてステータスをマップする
            var problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception?.Message,
                Instance = HttpContext?.Request?.Path
            };

            if (exception is ArgumentException)
            {
                problemDetails.Title = "Invalid request";
                problemDetails.Status = StatusCodes.Status400BadRequest;
            }
            else if (exception is KeyNotFoundException)
            {
                problemDetails.Title = "Resource not found";
                problemDetails.Status = StatusCodes.Status404NotFound;
            }
            else if (exception is UnauthorizedAccessException)
            {
                problemDetails.Title = "Unauthorized";
                problemDetails.Status = StatusCodes.Status401Unauthorized;
            }

            problemDetails.Extensions["traceId"] = traceId;

            _logger.LogError(exception, "Unhandled exception caught by global handler. TraceId: {TraceId}", traceId);

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }
    }
}