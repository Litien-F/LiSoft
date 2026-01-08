using LiSoft.Application.Models;

namespace LiSoft.Application.Services;

public interface IContactService
{
    Task<Contact> CreateContactAsync(ContactDto contactDto);
    Task<IEnumerable<Contact>> GetAllContactsAsync();
    Task<Contact?> GetContactByIdAsync(string id);
}
