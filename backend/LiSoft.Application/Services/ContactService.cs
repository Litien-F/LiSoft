using LiSoft.Application.Models;
using LiSoft.MongoDB.Configuration;
using LiSoft.MongoDB.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LiSoft.Application.Services;

public class ContactService : IContactService
{
    private readonly IMongoCollection<Contact> _contacts;
    private readonly ILogger<ContactService> _logger;

    public ContactService(
        IMongoDbService mongoDbService,
        IOptions<MongoDbSettings> settings,
        ILogger<ContactService> logger)
    {
        _logger = logger;
        var collectionName = settings.Value.Collections.GetValueOrDefault("Contacts", "contacts");
        _contacts = mongoDbService.GetCollection<Contact>(collectionName);
        
        _logger.LogDebug("ContactService inicializado com a coleção: {CollectionName}", collectionName);
    }

    public async Task<Contact> CreateContactAsync(ContactDto contactDto)
    {
        var contact = new Contact
        {
            Name = contactDto.Name,
            Email = contactDto.Email,
            Message = contactDto.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _contacts.InsertOneAsync(contact);
        return contact;
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        return await _contacts.Find(_ => true)
            .SortByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(string id)
    {
        return await _contacts.Find(c => c.Id == id).FirstOrDefaultAsync();
    }
}
