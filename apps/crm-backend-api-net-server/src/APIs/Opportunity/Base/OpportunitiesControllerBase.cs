using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class OpportunitiesControllerBase : ControllerBase
{
    protected readonly IOpportunitiesService _service;

    public OpportunitiesControllerBase(IOpportunitiesService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Opportunity
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<Opportunity>> CreateOpportunity(OpportunityCreateInput input)
    {
        var opportunity = await _service.CreateOpportunity(input);

        return CreatedAtAction(nameof(Opportunity), new { id = opportunity.Id }, opportunity);
    }

    /// <summary>
    /// Delete one Opportunity
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteOpportunity(
        [FromRoute()] OpportunityWhereUniqueInput uniqueId
    )
    {
        try
        {
            await _service.DeleteOpportunity(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Opportunities
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<Opportunity>>> Opportunities(
        [FromQuery()] OpportunityFindManyArgs filter
    )
    {
        return Ok(await _service.Opportunities(filter));
    }

    /// <summary>
    /// Meta data about Opportunity records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> OpportunitiesMeta(
        [FromQuery()] OpportunityFindManyArgs filter
    )
    {
        return Ok(await _service.OpportunitiesMeta(filter));
    }

    /// <summary>
    /// Get one Opportunity
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<Opportunity>> Opportunity(
        [FromRoute()] OpportunityWhereUniqueInput uniqueId
    )
    {
        try
        {
            return await _service.Opportunity(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Opportunity
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateOpportunity(
        [FromRoute()] OpportunityWhereUniqueInput uniqueId,
        [FromQuery()] OpportunityUpdateInput opportunityUpdateDto
    )
    {
        try
        {
            await _service.UpdateOpportunity(uniqueId, opportunityUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a Lead record for Opportunity
    /// </summary>
    [HttpGet("{Id}/lead")]
    public async Task<ActionResult<List<Lead>>> GetLead(
        [FromRoute()] OpportunityWhereUniqueInput uniqueId
    )
    {
        var lead = await _service.GetLead(uniqueId);
        return Ok(lead);
    }
}
