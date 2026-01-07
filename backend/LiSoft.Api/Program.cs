using LiSoft.Api.Models;
using LiSoft.Api.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB Configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");
if (string.IsNullOrWhiteSpace(mongoConnectionString))
{
    throw new InvalidOperationException(
        "A string de conexão do MongoDB não foi configurada. " +
        "Configure 'ConnectionStrings:MongoDB' no appsettings.json ou appsettings.Development.json");
}

const string mongoDatabaseName = "system_lisoft";
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

try
{
    mongoClient.GetDatabase("admin").RunCommand<MongoDB.Bson.BsonDocument>(
        new MongoDB.Bson.BsonDocument("ping", 1));
    
    builder.Services.AddSingleton<IMongoClient>(mongoClient);
    builder.Services.AddScoped<IMongoDatabase>(sp => mongoDatabase);
}
catch (Exception ex)
{
    throw new InvalidOperationException(
        $"Não foi possível conectar ao MongoDB. Verifique a string de conexão e se o MongoDB está rodando. " +
        $"Connection String: {mongoConnectionString}, Database: {mongoDatabaseName}. Erro: {ex.Message}", ex);
}

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Conexão com MongoDB estabelecida com sucesso. Database: {DatabaseName}", mongoDatabaseName);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
