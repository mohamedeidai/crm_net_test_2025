using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using CrmBackendApiNet.APIs.Extensions;
using CrmBackendApiNet.Infrastructure;
using CrmBackendApiNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CrmBackendApiNet.APIs;

public abstract class ContactsServiceBase : IContactsService
{
    protected readonly CrmBackendApiNetDbContext _context;

    public ContactsServiceBase(CrmBackendApiNetDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Contact
    /// </summary>
    public async Task<Contact> CreateContact(ContactCreateInput createDto)
    {
        var contact = new ContactDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Email = createDto.Email,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Phone = createDto.Phone,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            contact.Id = createDto.Id;
        }
        if (createDto.Customer != null)
        {
            contact.Customer = await _context
                .Customers.Where(customer => createDto.Customer.Id == customer.Id)
                .FirstOrDefaultAsync();
        }

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<ContactDbModel>(contact.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Contact
    /// </summary>
    public async Task DeleteContact(ContactWhereUniqueInput uniqueId)
    {
        var contact = await _context.Contacts.FindAsync(uniqueId.Id);
        if (contact == null)
        {
            throw new NotFoundException();
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Contacts
    /// </summary>
    public async Task<List<Contact>> Contacts(ContactFindManyArgs findManyArgs)
    {
        var contacts = await _context
            .Contacts.Include(x => x.Customer)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return contacts.ConvertAll(contact => contact.ToDto());
    }

    /// <summary>
    /// Meta data about Contact records
    /// </summary>
    public async Task<MetadataDto> ContactsMeta(ContactFindManyArgs findManyArgs)
    {
        var count = await _context.Contacts.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Contact
    /// </summary>
    public async Task<Contact> Contact(ContactWhereUniqueInput uniqueId)
    {
        var contacts = await this.Contacts(
            new ContactFindManyArgs { Where = new ContactWhereInput { Id = uniqueId.Id } }
        );
        var contact = contacts.FirstOrDefault();
        if (contact == null)
        {
            throw new NotFoundException();
        }

        return contact;
    }

    /// <summary>
    /// Update one Contact
    /// </summary>
    public async Task UpdateContact(ContactWhereUniqueInput uniqueId, ContactUpdateInput updateDto)
    {
        var contact = updateDto.ToModel(uniqueId);

        if (updateDto.Customer != null)
        {
            contact.Customer = await _context
                .Customers.Where(customer => updateDto.Customer == customer.Id)
                .FirstOrDefaultAsync();
        }

        _context.Entry(contact).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Contacts.Any(e => e.Id == contact.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Get a Customer record for Contact
    /// </summary>
    public async Task<Customer> GetCustomer(ContactWhereUniqueInput uniqueId)
    {
        var contact = await _context
            .Contacts.Where(contact => contact.Id == uniqueId.Id)
            .Include(contact => contact.Customer)
            .FirstOrDefaultAsync();
        if (contact == null)
        {
            throw new NotFoundException();
        }
        return contact.Customer.ToDto();
    }
}
