using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;

namespace CrmBackendApiNet.APIs;

public interface ICustomersService
{
    /// <summary>
    /// Create one Customer
    /// </summary>
    public Task<Customer> CreateCustomer(CustomerCreateInput customer);

    /// <summary>
    /// Delete one Customer
    /// </summary>
    public Task DeleteCustomer(CustomerWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Customers
    /// </summary>
    public Task<List<Customer>> Customers(CustomerFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Customer records
    /// </summary>
    public Task<MetadataDto> CustomersMeta(CustomerFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Customer
    /// </summary>
    public Task<Customer> Customer(CustomerWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Customer
    /// </summary>
    public Task UpdateCustomer(CustomerWhereUniqueInput uniqueId, CustomerUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Contacts records to Customer
    /// </summary>
    public Task ConnectContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactWhereUniqueInput[] contactsId
    );

    /// <summary>
    /// Disconnect multiple Contacts records from Customer
    /// </summary>
    public Task DisconnectContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactWhereUniqueInput[] contactsId
    );

    /// <summary>
    /// Find multiple Contacts records for Customer
    /// </summary>
    public Task<List<Contact>> FindContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactFindManyArgs ContactFindManyArgs
    );

    /// <summary>
    /// Update multiple Contacts records for Customer
    /// </summary>
    public Task UpdateContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactWhereUniqueInput[] contactsId
    );

    /// <summary>
    /// Connect multiple Leads records to Customer
    /// </summary>
    public Task ConnectLeads(CustomerWhereUniqueInput uniqueId, LeadWhereUniqueInput[] leadsId);

    /// <summary>
    /// Disconnect multiple Leads records from Customer
    /// </summary>
    public Task DisconnectLeads(CustomerWhereUniqueInput uniqueId, LeadWhereUniqueInput[] leadsId);

    /// <summary>
    /// Find multiple Leads records for Customer
    /// </summary>
    public Task<List<Lead>> FindLeads(
        CustomerWhereUniqueInput uniqueId,
        LeadFindManyArgs LeadFindManyArgs
    );

    /// <summary>
    /// Update multiple Leads records for Customer
    /// </summary>
    public Task UpdateLeads(CustomerWhereUniqueInput uniqueId, LeadWhereUniqueInput[] leadsId);
}
