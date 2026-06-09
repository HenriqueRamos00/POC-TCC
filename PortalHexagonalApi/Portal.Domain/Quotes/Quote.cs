namespace Portal.Domain.Quotes;

public sealed class Quote
{
    public string Id { get; }
    public string ProposalNumber { get; }
    public string CustomerName { get; }
    public string Description { get; }
    public decimal TotalValue { get; }
    public string Status { get; }

    public Quote(
        string id,
        string proposalNumber,
        string customerName,
        string description,
        decimal totalValue,
        string status)
    {
        Id = id;
        ProposalNumber = proposalNumber;
        CustomerName = customerName;
        Description = description;
        TotalValue = totalValue;
        Status = status;
    }
}