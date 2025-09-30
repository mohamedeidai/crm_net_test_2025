using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;

namespace CrmBackendApiNet.APIs;

public interface IContactsService
{
    /// <summary>
    /// Create one Contact
    /// </summary>
    public Task<Contact> CreateContact(ContactCreateInput contact);

    /// <summary>
    /// Delete one Contact
    /// </summary>
    public Task DeleteContact(ContactWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Contacts
    /// </summary>
    public Task<List<Contact>> Contacts(ContactFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Contact records
    /// </summary>
    public Task<MetadataDto> ContactsMeta(ContactFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Contact
    /// </summary>
    public Task<Contact> Contact(ContactWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Contact
    /// </summary>
    public Task UpdateContact(ContactWhereUniqueInput uniqueId, ContactUpdateInput updateDto);

    /// <summary>
    /// Get a Customer record for Contact
    /// </summary>
    public Task<Customer> GetCustomer(ContactWhereUniqueInput uniqueId);
}
