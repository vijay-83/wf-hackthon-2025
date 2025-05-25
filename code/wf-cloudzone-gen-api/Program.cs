using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.LangChainIntegration;
using wf_cloudzone_gen_api.Repositories;
using wf_cloudzone_gen_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    builder.Services.AddSingleton<Qdrant.Client.QdrantClient>(sp =>
    {
        var qdrantUrl = new Uri("http://localhost:6334");
        return new Qdrant.Client.QdrantClient(qdrantUrl);
    });
}
catch (UriFormatException ex)
{
    Console.WriteLine($"Invalid URI: {ex.Message}");
}

builder.Services.AddSingleton<EmbeddingService>();
builder.Services.AddSingleton<QdrantService>();
// Inject custom services
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IAgentService, AgentService>();

// Inject repositories (MockServiceNow & Qdrant)
builder.Services.AddScoped<IMockServiceNowClient, MockServiceNowClient>();
builder.Services.AddScoped<IQdrantRepository, QdrantRepository>();

// Inject HttpClient for external API calls (LangChain, ServiceNow, Jira)
builder.Services.AddHttpClient<ILangChainClient, LangChainClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
