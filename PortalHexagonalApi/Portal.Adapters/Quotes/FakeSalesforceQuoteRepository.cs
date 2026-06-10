using Portal.Application.Quotes;
using Portal.Domain.Quotes;

namespace Portal.Adapters.Quotes;

public sealed class FakeSalesforceQuoteRepository : IQuoteRepository
{
    private static readonly List<FakeQuote> Quotes =
    [
        new FakeQuote(
            Id: "quote-1",
            Quote: new Quote(
                name: "EAQ_2026_45482_V_1",
                status: "Pendente de Aceite",
                description: "Análise físico-química de óleo isolante",
                totalPrice: 56292.30m)
        ),
        new FakeQuote(
            Id: "quote-2",
            Quote: new Quote(
                name: "EAQ_2026_45483_V_1",
                status: "Em Andamento",
                description: "Ensaios laboratoriais de materiais",
                totalPrice: 12500.00m)
        )
    ];

    public Task<IReadOnlyList<Quote>> GetQuotesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<Quote>>(Quotes.Select(q => q.Quote).ToList());
    }

    public Task<Quote?> GetQuoteByIdAsync(string id, CancellationToken cancellationToken)
    {
        Quote? quote = Quotes.FirstOrDefault(q => q.Id == id)?.Quote;
        return Task.FromResult(quote);
    }

    private sealed record FakeQuote(string Id, Quote Quote);
}
