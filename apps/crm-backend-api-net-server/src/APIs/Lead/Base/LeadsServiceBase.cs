using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using CrmBackendApiNet.APIs.Extensions;
using CrmBackendApiNet.Infrastructure;
using CrmBackendApiNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CrmBackendApiNet.APIs;

public abstract class LeadsServiceBase : ILeadsService
{
    protected readonly CrmBackendApiNetDbContext _context;

    public LeadsServiceBase(CrmBackendApiNetDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Lead
    /// </summary>
    public async Task<Lead> CreateLead(LeadCreateInput createDto)
    {
        var lead = new LeadDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Email = createDto.Email,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Status = createDto.Status,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            lead.Id = createDto.Id;
        }
        if (createDto.Customer != null)
        {
            lead.Customer = await _context
                .Customers.Where(customer => createDto.Customer.Id == customer.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Opportunities != null)
        {
            lead.Opportunities = await _context
                .Opportunities.Where(opportunity =>
                    createDto.Opportunities.Select(t => t.Id).Contains(opportunity.Id)
                )
                .ToListAsync();
        }

        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<LeadDbModel>(lead.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Lead
    /// </summary>
    public async Task DeleteLead(LeadWhereUniqueInput uniqueId)
    {
        var lead = await _context.Leads.FindAsync(uniqueId.Id);
        if (lead == null)
        {
            throw new NotFoundException();
        }

        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Leads
    /// </summary>
    public async Task<List<Lead>> Leads(LeadFindManyArgs findManyArgs)
    {
        var leads = await _context
            .Leads.Include(x => x.Customer)
            .Include(x => x.Opportunities)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return leads.ConvertAll(lead => lead.ToDto());
    }

    /// <summary>
    /// Meta data about Lead records
    /// </summary>
    public async Task<MetadataDto> LeadsMeta(LeadFindManyArgs findManyArgs)
    {
        var count = await _context.Leads.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Lead
    /// </summary>
    public async Task<Lead> Lead(LeadWhereUniqueInput uniqueId)
    {
        var leads = await this.Leads(
            new LeadFindManyArgs { Where = new LeadWhereInput { Id = uniqueId.Id } }
        );
        var lead = leads.FirstOrDefault();
        if (lead == null)
        {
            throw new NotFoundException();
        }

        return lead;
    }

    /// <summary>
    /// Update one Lead
    /// </summary>
    public async Task UpdateLead(LeadWhereUniqueInput uniqueId, LeadUpdateInput updateDto)
    {
        var lead = updateDto.ToModel(uniqueId);

        if (updateDto.Customer != null)
        {
            lead.Customer = await _context
                .Customers.Where(customer => updateDto.Customer == customer.Id)
                .FirstOrDefaultAsync();
        }

        if (updateDto.Opportunities != null)
        {
            lead.Opportunities = await _context
                .Opportunities.Where(opportunity =>
                    updateDto.Opportunities.Select(t => t).Contains(opportunity.Id)
                )
                .ToListAsync();
        }

        _context.Entry(lead).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Leads.Any(e => e.Id == lead.Id))
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
    /// Get a Customer record for Lead
    /// </summary>
    public async Task<Customer> GetCustomer(LeadWhereUniqueInput uniqueId)
    {
        var lead = await _context
            .Leads.Where(lead => lead.Id == uniqueId.Id)
            .Include(lead => lead.Customer)
            .FirstOrDefaultAsync();
        if (lead == null)
        {
            throw new NotFoundException();
        }
        return lead.Customer.ToDto();
    }

    /// <summary>
    /// Connect multiple Opportunities records to Lead
    /// </summary>
    public async Task ConnectOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Leads.Include(x => x.Opportunities)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Opportunities.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        var childrenToConnect = children.Except(parent.Opportunities);

        foreach (var child in childrenToConnect)
        {
            parent.Opportunities.Add(child);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Opportunities records from Lead
    /// </summary>
    public async Task DisconnectOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Leads.Include(x => x.Opportunities)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Opportunities.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var child in children)
        {
            parent.Opportunities?.Remove(child);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Opportunities records for Lead
    /// </summary>
    public async Task<List<Opportunity>> FindOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityFindManyArgs leadFindManyArgs
    )
    {
        var opportunities = await _context
            .Opportunities.Where(m => m.LeadId == uniqueId.Id)
            .ApplyWhere(leadFindManyArgs.Where)
            .ApplySkip(leadFindManyArgs.Skip)
            .ApplyTake(leadFindManyArgs.Take)
            .ApplyOrderBy(leadFindManyArgs.SortBy)
            .ToListAsync();

        return opportunities.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Opportunities records for Lead
    /// </summary>
    public async Task UpdateOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityWhereUniqueInput[] childrenIds
    )
    {
        var lead = await _context
            .Leads.Include(t => t.Opportunities)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (lead == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Opportunities.Where(a => childrenIds.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        lead.Opportunities = children;
        await _context.SaveChangesAsync();
    }
}
