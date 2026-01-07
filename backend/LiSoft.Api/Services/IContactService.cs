using LiSoft.Api.Models;

namespace LiSoft.Api.Services;

public interface IContactService
{
    Task<Contact> CreateContactAsync(ContactDto contactDto);
    Task<IEnumerable<Contact>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(string id);
}
