using Microsoft.AspNetCore.Mvc;
using Portal.Application.LabResults;
using Portal.Domain.LabResults;

namespace Portal.Api.Controllers;

[ApiController]
[Route("api/lab-results")]
public sealed class LabResultsController : ControllerBase
{
    private readonly LabResultService _labResultService;

    public LabResultsController(LabResultService labResultService)
    {
        _labResultService = labResultService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LabResult>>> GetLabResults(CancellationToken cancellationToken)
    {
        IReadOnlyList<LabResult> results = await _labResultService.GetLabResultsAsync(cancellationToken);
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LabResult>> GetLabResultById(string id, CancellationToken cancellationToken)
    {
        LabResult? result = await _labResultService.GetLabResultByIdAsync(id, cancellationToken);

        if (result is null)
            return NotFound(new { message = "Lab result not found." });

        return Ok(result);
    }
}