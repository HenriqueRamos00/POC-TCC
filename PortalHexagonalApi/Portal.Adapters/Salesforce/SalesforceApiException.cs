namespace Portal.Adapters.Salesforce;

public sealed class SalesforceApiException : Exception
{
    public SalesforceApiException(string message)
        : base(message)
    {
    }

    public SalesforceApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
