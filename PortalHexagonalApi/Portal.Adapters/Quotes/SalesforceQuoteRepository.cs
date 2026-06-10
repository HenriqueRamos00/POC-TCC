using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Portal.Adapters.Salesforce;
using Portal.Application.Quotes;
using Portal.Domain.Quotes;

namespace Portal.Adapters.Quotes;

public sealed class SalesforceQuoteRepository : IQuoteRepository
{
    private const string QuoteFields = "Id, Name, Status, Opportunity.Account.Name, Descricao__c, TotalPrice";
    private const string LatestQuotesQuery = $"SELECT {QuoteFields} FROM Quote ORDER BY CreatedDate DESC LIMIT 5";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;
    private readonly ISalesforceTokenProvider _tokenProvider;
    private readonly SalesforceOptions _options;

    public SalesforceQuoteRepository(
        HttpClient httpClient,
        ISalesforceTokenProvider tokenProvider,
        SalesforceOptions options)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
        _options = options;
    }

    public async Task<IReadOnlyList<Quote>> GetQuotesAsync(CancellationToken cancellationToken)
    {
        SalesforceQueryResponse response = await QuerySalesforceAsync(LatestQuotesQuery, cancellationToken);
        return response.Records.Select(MapQuote).ToList();
    }

    public async Task<Quote?> GetQuoteByIdAsync(string id, CancellationToken cancellationToken)
    {
        string escapedId = EscapeSoqlString(id);
        string query = $"SELECT {QuoteFields} FROM Quote WHERE Id = '{escapedId}' LIMIT 1";

        SalesforceQueryResponse response = await QuerySalesforceAsync(query, cancellationToken);
        SalesforceQuoteRecord? record = response.Records.FirstOrDefault();

        return record is null ? null : MapQuote(record);
    }

    private async Task<SalesforceQueryResponse> QuerySalesforceAsync(string query, CancellationToken cancellationToken)
    {
        SalesforceToken token = await _tokenProvider.GetTokenAsync(cancellationToken);
        HttpResponseMessage response = await SendQueryAsync(query, token, cancellationToken);

        try
        {
            if (IsAuthFailure(response.StatusCode))
            {
                response.Dispose();

                token = await _tokenProvider.RefreshTokenAsync(cancellationToken);
                response = await SendQueryAsync(query, token, cancellationToken);
            }

            await using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using StreamReader reader = new(responseStream);
                string responseContent = await reader.ReadToEndAsync(cancellationToken);

                throw new SalesforceApiException(
                    $"Salesforce query failed with {(int)response.StatusCode} {response.ReasonPhrase}: {responseContent}");
            }

            SalesforceQueryResponse? queryResponse = await JsonSerializer.DeserializeAsync<SalesforceQueryResponse>(
                responseStream,
                JsonOptions,
                cancellationToken);

            return queryResponse ?? new SalesforceQueryResponse();
        }
        finally
        {
            response.Dispose();
        }
    }

    private async Task<HttpResponseMessage> SendQueryAsync(
        string query,
        SalesforceToken token,
        CancellationToken cancellationToken)
    {
        using HttpRequestMessage request = new(HttpMethod.Get, BuildQueryUri(token, query));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue(
            string.IsNullOrWhiteSpace(token.TokenType) ? "Bearer" : token.TokenType,
            token.AccessToken);

        return await _httpClient.SendAsync(request, cancellationToken);
    }

    private Uri BuildQueryUri(SalesforceToken token, string query)
    {
        string baseUrl = string.IsNullOrWhiteSpace(token.InstanceUrl) ? _options.Url : token.InstanceUrl;

        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? salesforceUrl))
            throw new InvalidOperationException("Salesforce base URL must be an absolute URL.");

        string apiVersion = string.IsNullOrWhiteSpace(_options.ApiVersion) ? "v60.0" : _options.ApiVersion;
        Uri queryUri = new(salesforceUrl, $"services/data/{apiVersion}/query/");
        UriBuilder uriBuilder = new(queryUri)
        {
            Query = $"q={Uri.EscapeDataString(query)}"
        };

        return uriBuilder.Uri;
    }

    private static bool IsAuthFailure(HttpStatusCode statusCode)
    {
        return statusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden;
    }

    private static Quote MapQuote(SalesforceQuoteRecord record)
    {
        return new Quote(
            name: record.Name ?? string.Empty,
            status: record.Status ?? string.Empty,
            description: record.Description ?? string.Empty,
            totalPrice: record.TotalPrice ?? 0m);
    }

    private static string EscapeSoqlString(string value)
    {
        return value.Replace("\\", "\\\\").Replace("'", "\\'");
    }

    private sealed class SalesforceQueryResponse
    {
        [JsonPropertyName("records")]
        public List<SalesforceQuoteRecord> Records { get; set; } = [];
    }

    private sealed class SalesforceQuoteRecord
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        [JsonPropertyName("Descricao__c")]
        public string? Description { get; set; }

        [JsonPropertyName("TotalPrice")]
        public decimal? TotalPrice { get; set; }
    }
}
