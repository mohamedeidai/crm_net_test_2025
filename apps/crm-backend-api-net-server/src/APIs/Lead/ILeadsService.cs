using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;

namespace CrmBackendApiNet.APIs;

public interface ILeadsService
{
    /// <summary>
    /// Create one Lead
    /// </summary>
    public Task<Lead> CreateLead(LeadCreateInput lead);

    /// <summary>
    /// Delete one Lead
    /// </summary>
    public Task DeleteLead(LeadWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Leads
    /// </summary>
    public Task<List<Lead>> Leads(LeadFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Lead records
    /// </summary>
    public Task<MetadataDto> LeadsMeta(LeadFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Lead
    /// </summary>
    public Task<Lead> Lead(LeadWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Lead
    /// </summary>
    public Task UpdateLead(LeadWhereUniqueInput uniqueId, LeadUpdateInput updateDto);

    /// <summary>
    /// Get a Customer record for Lead
    /// </summary>
    public Task<Customer> GetCustomer(LeadWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Opportunities records to Lead
    /// </summary>
    public Task ConnectOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityWhereUniqueInput[] opportunitiesId
    );

    /// <summary>
    /// Disconnect multiple Opportunities records from Lead
    /// </summary>
    public Task DisconnectOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityWhereUniqueInput[] opportunitiesId
    );

    /// <summary>
    /// Find multiple Opportunities records for Lead
    /// </summary>
    public Task<List<Opportunity>> FindOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityFindManyArgs OpportunityFindManyArgs
    );

    /// <summary>
    /// Update multiple Opportunities records for Lead
    /// </summary>
    public Task UpdateOpportunities(
        LeadWhereUniqueInput uniqueId,
        OpportunityWhereUniqueInput[] opportunitiesId
    );
}
