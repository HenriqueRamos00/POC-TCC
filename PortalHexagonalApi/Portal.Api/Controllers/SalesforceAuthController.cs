using Microsoft.AspNetCore.Mvc;
using Portal.Adapters.Salesforce;

namespace Portal.Api.Controllers;

[ApiController]
[Route("api/salesforce")]
public sealed class SalesforceAuthController : ControllerBase
{
    private readonly ISalesforceTokenClient _salesforceTokenClient;

    public SalesforceAuthController(ISalesforceTokenClient salesforceTokenClient)
    {
        _salesforceTokenClient = salesforceTokenClient;
    }

    [HttpPost("token")]
    public async Task<ActionResult<SalesforceToken>> GetToken(CancellationToken cancellationToken)
    {
        try
        {
            SalesforceToken token = await _salesforceTokenClient.GetTokenAsync(cancellationToken);
            return Ok(token);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (HttpRequestException exception)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { message = exception.Message });
        }
    }
}
