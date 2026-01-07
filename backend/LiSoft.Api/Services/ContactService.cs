using LiSoft.Api.Models;
using MongoDB.Driver;

namespace LiSoft.Api.Services;

public class ContactService : IContactService
{
    private readonly IMongoCollection<Contact> _contacts;

    public ContactService(IMongoDatabase database)
    {
        _contacts = database.GetCollection<Contact>("contacts");
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
