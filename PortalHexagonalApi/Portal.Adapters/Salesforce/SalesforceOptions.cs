namespace Portal.Adapters.Salesforce;

public sealed class SalesforceOptions
{
    public const string SectionName = "Salesforce";

    public string Url { get; set; } = string.Empty;
    public string TokenPath { get; set; } = "/services/oauth2/token";
    public string GrantType { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
