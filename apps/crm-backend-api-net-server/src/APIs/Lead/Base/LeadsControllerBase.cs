using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class LeadsControllerBase : ControllerBase
{
    protected readonly ILeadsService _service;

    public LeadsControllerBase(ILeadsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Lead
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<Lead>> CreateLead(LeadCreateInput input)
    {
        var lead = await _service.CreateLead(input);

        return CreatedAtAction(nameof(Lead), new { id = lead.Id }, lead);
    }

    /// <summary>
    /// Delete one Lead
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteLead([FromRoute()] LeadWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteLead(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Leads
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<Lead>>> Leads([FromQuery()] LeadFindManyArgs filter)
    {
        return Ok(await _service.Leads(filter));
    }

    /// <summary>
    /// Meta data about Lead records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> LeadsMeta([FromQuery()] LeadFindManyArgs filter)
    {
        return Ok(await _service.LeadsMeta(filter));
    }

    /// <summary>
    /// Get one Lead
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<Lead>> Lead([FromRoute()] LeadWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Lead(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Lead
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateLead(
        [FromRoute()] LeadWhereUniqueInput uniqueId,
        [FromQuery()] LeadUpdateInput leadUpdateDto
    )
    {
        try
        {
            await _service.UpdateLead(uniqueId, leadUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a Customer record for Lead
    /// </summary>
    [HttpGet("{Id}/customer")]
    public async Task<ActionResult<List<Customer>>> GetCustomer(
        [FromRoute()] LeadWhereUniqueInput uniqueId
    )
    {
        var customer = await _service.GetCustomer(uniqueId);
        return Ok(customer);
    }

    /// <summary>
    /// Connect multiple Opportunities records to Lead
    /// </summary>
    [HttpPost("{Id}/opportunities")]
    public async Task<ActionResult> ConnectOpportunities(
        [FromRoute()] LeadWhereUniqueInput uniqueId,
        [FromQuery()] OpportunityWhereUniqueInput[] opportunitiesId
    )
    {
        try
        {
            await _service.ConnectOpportunities(uniqueId, opportunitiesId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Opportunities records from Lead
    /// </summary>
    [HttpDelete("{Id}/opportunities")]
    public async Task<ActionResult> DisconnectOpportunities(
        [FromRoute()] LeadWhereUniqueInput uniqueId,
        [FromBody()] OpportunityWhereUniqueInput[] opportunitiesId
    )
    {
        try
        {
            await _service.DisconnectOpportunities(uniqueId, opportunitiesId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Opportunities records for Lead
    /// </summary>
    [HttpGet("{Id}/opportunities")]
    public async Task<ActionResult<List<Opportunity>>> FindOpportunities(
        [FromRoute()] LeadWhereUniqueInput uniqueId,
        [FromQuery()] OpportunityFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindOpportunities(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Opportunities records for Lead
    /// </summary>
    [HttpPatch("{Id}/opportunities")]
    public async Task<ActionResult> UpdateOpportunities(
        [FromRoute()] LeadWhereUniqueInput uniqueId,
        [FromBody()] OpportunityWhereUniqueInput[] opportunitiesId
    )
    {
        try
        {
            await _service.UpdateOpportunities(uniqueId, opportunitiesId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
