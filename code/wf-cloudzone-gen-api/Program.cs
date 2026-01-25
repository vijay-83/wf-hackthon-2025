using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.LangChainIntegration;
using wf_cloudzone_gen_api.LangChainIntegration.Summary;
using wf_cloudzone_gen_api.Repositories;
using wf_cloudzone_gen_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

// Inject repositories (MockServiceNow & Qdrant)
builder.Services.AddSingleton<QdrantService>();
builder.Services.AddScoped<IMockServiceNowClient, MockServiceNowClient>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddSingleton<IIncidentIndexerService, IncidentIndexerService>();
builder.Services.AddSingleton<IEmbeddingService, EmbeddingService>();

// Inject HttpClient for external API calls (LangChain, ServiceNow, Jira)
builder.Services.AddHttpClient<LangChainClient>();
builder.Services.AddScoped<ISummaryAgent, SummaryAgent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontendDev");

app.UseAuthorization();

app.MapControllers();

app.Run();
