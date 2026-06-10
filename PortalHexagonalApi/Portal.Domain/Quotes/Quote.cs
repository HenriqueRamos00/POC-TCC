namespace Portal.Domain.Quotes;

public sealed class Quote
{
    public string Name { get; }
    public string Status { get; }
    public string Description { get; }
    public decimal TotalPrice { get; }

    public Quote(
        string name,
        string status,
        string description,
        decimal totalPrice)
    {
        Name = name;
        Status = status;
        Description = description;
        TotalPrice = totalPrice;
    }
}
