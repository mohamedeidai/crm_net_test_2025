using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using CrmBackendApiNet.APIs.Extensions;
using CrmBackendApiNet.Infrastructure;
using CrmBackendApiNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CrmBackendApiNet.APIs;

public abstract class OpportunitiesServiceBase : IOpportunitiesService
{
    protected readonly CrmBackendApiNetDbContext _context;

    public OpportunitiesServiceBase(CrmBackendApiNetDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Opportunity
    /// </summary>
    public async Task<Opportunity> CreateOpportunity(OpportunityCreateInput createDto)
    {
        var opportunity = new OpportunityDbModel
        {
            Amount = createDto.Amount,
            CreatedAt = createDto.CreatedAt,
            Stage = createDto.Stage,
            Title = createDto.Title,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            opportunity.Id = createDto.Id;
        }
        if (createDto.Lead != null)
        {
            opportunity.Lead = await _context
                .Leads.Where(lead => createDto.Lead.Id == lead.Id)
                .FirstOrDefaultAsync();
        }

        _context.Opportunities.Add(opportunity);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<OpportunityDbModel>(opportunity.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Opportunity
    /// </summary>
    public async Task DeleteOpportunity(OpportunityWhereUniqueInput uniqueId)
    {
        var opportunity = await _context.Opportunities.FindAsync(uniqueId.Id);
        if (opportunity == null)
        {
            throw new NotFoundException();
        }

        _context.Opportunities.Remove(opportunity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Opportunities
    /// </summary>
    public async Task<List<Opportunity>> Opportunities(OpportunityFindManyArgs findManyArgs)
    {
        var opportunities = await _context
            .Opportunities.Include(x => x.Lead)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return opportunities.ConvertAll(opportunity => opportunity.ToDto());
    }

    /// <summary>
    /// Meta data about Opportunity records
    /// </summary>
    public async Task<MetadataDto> OpportunitiesMeta(OpportunityFindManyArgs findManyArgs)
    {
        var count = await _context.Opportunities.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Opportunity
    /// </summary>
    public async Task<Opportunity> Opportunity(OpportunityWhereUniqueInput uniqueId)
    {
        var opportunities = await this.Opportunities(
            new OpportunityFindManyArgs { Where = new OpportunityWhereInput { Id = uniqueId.Id } }
        );
        var opportunity = opportunities.FirstOrDefault();
        if (opportunity == null)
        {
            throw new NotFoundException();
        }

        return opportunity;
    }

    /// <summary>
    /// Update one Opportunity
    /// </summary>
    public async Task UpdateOpportunity(
        OpportunityWhereUniqueInput uniqueId,
        OpportunityUpdateInput updateDto
    )
    {
        var opportunity = updateDto.ToModel(uniqueId);

        if (updateDto.Lead != null)
        {
            opportunity.Lead = await _context
                .Leads.Where(lead => updateDto.Lead == lead.Id)
                .FirstOrDefaultAsync();
        }

        _context.Entry(opportunity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Opportunities.Any(e => e.Id == opportunity.Id))
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
    /// Get a Lead record for Opportunity
    /// </summary>
    public async Task<Lead> GetLead(OpportunityWhereUniqueInput uniqueId)
    {
        var opportunity = await _context
            .Opportunities.Where(opportunity => opportunity.Id == uniqueId.Id)
            .Include(opportunity => opportunity.Lead)
            .FirstOrDefaultAsync();
        if (opportunity == null)
        {
            throw new NotFoundException();
        }
        return opportunity.Lead.ToDto();
    }
}
