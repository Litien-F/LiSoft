using LiSoft.MongoDB.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LiSoft.MongoDB.Services;

public interface IMongoDbService
{
    IMongoDatabase Database { get; }
    IMongoCollection<T> GetCollection<T>(string collectionName);
}

public class MongoDbService : IMongoDbService
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDbService> _logger;
    private readonly MongoDbSettings _settings;

    public MongoDbService(
        IOptions<MongoDbSettings> settings,
        ILogger<MongoDbService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            throw new InvalidOperationException(
                "A string de conexão do MongoDB não foi configurada. " +
                $"Configure '{MongoDbSettings.SectionName}:ConnectionString' no appsettings.json");
        }

        if (string.IsNullOrWhiteSpace(_settings.DatabaseName))
        {
            throw new InvalidOperationException(
                "O nome do banco de dados MongoDB não foi configurado. " +
                $"Configure '{MongoDbSettings.SectionName}:DatabaseName' no appsettings.json");
        }

        try
        {
            _mongoClient = new MongoClient(_settings.ConnectionString);
            _database = _mongoClient.GetDatabase(_settings.DatabaseName);

            // Testa a conexão
            _database.RunCommand<global::MongoDB.Bson.BsonDocument>(
                new global::MongoDB.Bson.BsonDocument("ping", 1));

            _logger.LogInformation(
                "Conexão com MongoDB estabelecida com sucesso. Database: {DatabaseName}",
                _settings.DatabaseName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao conectar ao MongoDB. ConnectionString: {ConnectionString}, Database: {DatabaseName}",
                MaskConnectionString(_settings.ConnectionString),
                _settings.DatabaseName);
            throw new InvalidOperationException(
                $"Não foi possível conectar ao MongoDB. Verifique a string de conexão e se o MongoDB está acessível. " +
                $"Database: {_settings.DatabaseName}. Erro: {ex.Message}", ex);
        }
    }

    public IMongoDatabase Database => _database;

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
        {
            throw new ArgumentException("O nome da coleção não pode ser vazio.", nameof(collectionName));
        }

        return _database.GetCollection<T>(collectionName);
    }

    private static string MaskConnectionString(string connectionString)
    {
        // Mascara a senha na connection string para logs
        if (string.IsNullOrWhiteSpace(connectionString))
            return string.Empty;

        var parts = connectionString.Split('@');
        if (parts.Length < 2)
            return connectionString;

        var credentials = parts[0].Split("://");
        if (credentials.Length < 2)
            return connectionString;

        return $"{credentials[0]}://***:***@{parts[1]}";
    }
}
