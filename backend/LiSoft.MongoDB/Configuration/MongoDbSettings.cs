namespace LiSoft.MongoDB.Configuration;

public class MongoDbSettings
{
    public const string SectionName = "MongoDbSettings";
    
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    
    public Dictionary<string, string> Collections { get; set; } = new();
}
