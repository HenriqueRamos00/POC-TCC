using Portal.Application.Quotes;
using Portal.Domain.Quotes;

namespace Portal.Adapters.Quotes;

public sealed class FakeSalesforceQuoteRepository : IQuoteRepository
{
    private static readonly List<Quote> Quotes =
    [
        new Quote(
            id: "quote-1",
            proposalNumber: "EAQ_2026_45482_V_1",
            customerName: "Copel Distribuição",
            description: "Análise físico-química de óleo isolante",
            totalValue: 56292.30m,
            status: "Pendente de Aceite"
        ),
        new Quote(
            id: "quote-2",
            proposalNumber: "EAQ_2026_45483_V_1",
            customerName: "Petrobras",
            description: "Ensaios laboratoriais de materiais",
            totalValue: 12500.00m,
            status: "Em Andamento"
        )
    ];

    public Task<IReadOnlyList<Quote>> GetQuotesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyList<Quote>>(Quotes);
    }

    public Task<Quote?> GetQuoteByIdAsync(string id, CancellationToken cancellationToken)
    {
        Quote? quote = Quotes.FirstOrDefault(q => q.Id == id);
        return Task.FromResult(quote);
    }
}