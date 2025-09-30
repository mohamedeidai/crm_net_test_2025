using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using CrmBackendApiNet.APIs.Extensions;
using CrmBackendApiNet.Infrastructure;
using CrmBackendApiNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CrmBackendApiNet.APIs;

public abstract class CustomersServiceBase : ICustomersService
{
    protected readonly CrmBackendApiNetDbContext _context;

    public CustomersServiceBase(CrmBackendApiNetDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Customer
    /// </summary>
    public async Task<Customer> CreateCustomer(CustomerCreateInput createDto)
    {
        var customer = new CustomerDbModel
        {
            Address = createDto.Address,
            CreatedAt = createDto.CreatedAt,
            Email = createDto.Email,
            Name = createDto.Name,
            Phone = createDto.Phone,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            customer.Id = createDto.Id;
        }
        if (createDto.Contacts != null)
        {
            customer.Contacts = await _context
                .Contacts.Where(contact =>
                    createDto.Contacts.Select(t => t.Id).Contains(contact.Id)
                )
                .ToListAsync();
        }

        if (createDto.Leads != null)
        {
            customer.Leads = await _context
                .Leads.Where(lead => createDto.Leads.Select(t => t.Id).Contains(lead.Id))
                .ToListAsync();
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<CustomerDbModel>(customer.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Customer
    /// </summary>
    public async Task DeleteCustomer(CustomerWhereUniqueInput uniqueId)
    {
        var customer = await _context.Customers.FindAsync(uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Customers
    /// </summary>
    public async Task<List<Customer>> Customers(CustomerFindManyArgs findManyArgs)
    {
        var customers = await _context
            .Customers.Include(x => x.Leads)
            .Include(x => x.Contacts)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return customers.ConvertAll(customer => customer.ToDto());
    }

    /// <summary>
    /// Meta data about Customer records
    /// </summary>
    public async Task<MetadataDto> CustomersMeta(CustomerFindManyArgs findManyArgs)
    {
        var count = await _context.Customers.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Customer
    /// </summary>
    public async Task<Customer> Customer(CustomerWhereUniqueInput uniqueId)
    {
        var customers = await this.Customers(
            new CustomerFindManyArgs { Where = new CustomerWhereInput { Id = uniqueId.Id } }
        );
        var customer = customers.FirstOrDefault();
        if (customer == null)
        {
            throw new NotFoundException();
        }

        return customer;
    }

    /// <summary>
    /// Update one Customer
    /// </summary>
    public async Task UpdateCustomer(
        CustomerWhereUniqueInput uniqueId,
        CustomerUpdateInput updateDto
    )
    {
        var customer = updateDto.ToModel(uniqueId);

        if (updateDto.Contacts != null)
        {
            customer.Contacts = await _context
                .Contacts.Where(contact => updateDto.Contacts.Select(t => t).Contains(contact.Id))
                .ToListAsync();
        }

        if (updateDto.Leads != null)
        {
            customer.Leads = await _context
                .Leads.Where(lead => updateDto.Leads.Select(t => t).Contains(lead.Id))
                .ToListAsync();
        }

        _context.Entry(customer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Customers.Any(e => e.Id == customer.Id))
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
    /// Connect multiple Contacts records to Customer
    /// </summary>
    public async Task ConnectContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Contacts.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        var childrenToConnect = children.Except(parent.Contacts);

        foreach (var child in childrenToConnect)
        {
            parent.Contacts.Add(child);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Contacts records from Customer
    /// </summary>
    public async Task DisconnectContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Contacts.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var child in children)
        {
            parent.Contacts?.Remove(child);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Contacts records for Customer
    /// </summary>
    public async Task<List<Contact>> FindContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactFindManyArgs customerFindManyArgs
    )
    {
        var contacts = await _context
            .Contacts.Where(m => m.CustomerId == uniqueId.Id)
            .ApplyWhere(customerFindManyArgs.Where)
            .ApplySkip(customerFindManyArgs.Skip)
            .ApplyTake(customerFindManyArgs.Take)
            .ApplyOrderBy(customerFindManyArgs.SortBy)
            .ToListAsync();

        return contacts.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Contacts records for Customer
    /// </summary>
    public async Task UpdateContacts(
        CustomerWhereUniqueInput uniqueId,
        ContactWhereUniqueInput[] childrenIds
    )
    {
        var customer = await _context
            .Customers.Include(t => t.Contacts)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Contacts.Where(a => childrenIds.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        customer.Contacts = children;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple Leads records to Customer
    /// </summary>
    public async Task ConnectLeads(
        CustomerWhereUniqueInput uniqueId,
        LeadWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Leads)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Leads.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        var childrenToConnect = children.Except(parent.Leads);

        foreach (var child in childrenToConnect)
        {
            parent.Leads.Add(child);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Leads records from Customer
    /// </summary>
    public async Task DisconnectLeads(
        CustomerWhereUniqueInput uniqueId,
        LeadWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Customers.Include(x => x.Leads)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Leads.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var child in children)
        {
            parent.Leads?.Remove(child);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Leads records for Customer
    /// </summary>
    public async Task<List<Lead>> FindLeads(
        CustomerWhereUniqueInput uniqueId,
        LeadFindManyArgs customerFindManyArgs
    )
    {
        var leads = await _context
            .Leads.Where(m => m.CustomerId == uniqueId.Id)
            .ApplyWhere(customerFindManyArgs.Where)
            .ApplySkip(customerFindManyArgs.Skip)
            .ApplyTake(customerFindManyArgs.Take)
            .ApplyOrderBy(customerFindManyArgs.SortBy)
            .ToListAsync();

        return leads.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Leads records for Customer
    /// </summary>
    public async Task UpdateLeads(
        CustomerWhereUniqueInput uniqueId,
        LeadWhereUniqueInput[] childrenIds
    )
    {
        var customer = await _context
            .Customers.Include(t => t.Leads)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (customer == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Leads.Where(a => childrenIds.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        customer.Leads = children;
        await _context.SaveChangesAsync();
    }
}
