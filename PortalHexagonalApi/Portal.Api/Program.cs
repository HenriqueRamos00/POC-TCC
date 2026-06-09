using Portal.Adapters.LabResults;
using Portal.Adapters.Quotes;
using Portal.Application.LabResults;
using Portal.Application.Quotes;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<QuoteService>();
builder.Services.AddScoped<LabResultService>();

builder.Services.AddScoped<IQuoteRepository, FakeSalesforceQuoteRepository>();
builder.Services.AddScoped<ILabResultRepository, FakeSqlServerLabResultRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.MapOpenApi();

app.MapControllers();

app.Run();
