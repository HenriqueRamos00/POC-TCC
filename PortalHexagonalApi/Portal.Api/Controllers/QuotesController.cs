using Microsoft.AspNetCore.Mvc;
using Portal.Application.Quotes;
using Portal.Domain.Quotes;

namespace Portal.Api.Controllers;

[ApiController]
[Route("api/quotes")]
public sealed class QuotesController : ControllerBase
{
    private readonly IQuoteUseCase _quoteUseCase;

    public QuotesController(IQuoteUseCase quoteUseCase)
    {
        _quoteUseCase = quoteUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Quote>>> GetQuotes(CancellationToken cancellationToken)
    {
        IReadOnlyList<Quote> quotes = await _quoteUseCase.GetQuotesAsync(cancellationToken);
        return Ok(quotes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Quote>> GetQuoteById(string id, CancellationToken cancellationToken)
    {
        Quote? quote = await _quoteUseCase.GetQuoteByIdAsync(id, cancellationToken);

        if (quote is null)
            return NotFound(new { message = "Quote not found." });

        return Ok(quote);
    }
}
