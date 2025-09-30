using CrmBackendApiNet.APIs;
using CrmBackendApiNet.APIs.Common;
using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class ContactsControllerBase : ControllerBase
{
    protected readonly IContactsService _service;

    public ContactsControllerBase(IContactsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Contact
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<Contact>> CreateContact(ContactCreateInput input)
    {
        var contact = await _service.CreateContact(input);

        return CreatedAtAction(nameof(Contact), new { id = contact.Id }, contact);
    }

    /// <summary>
    /// Delete one Contact
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteContact([FromRoute()] ContactWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteContact(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Contacts
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<Contact>>> Contacts(
        [FromQuery()] ContactFindManyArgs filter
    )
    {
        return Ok(await _service.Contacts(filter));
    }

    /// <summary>
    /// Meta data about Contact records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> ContactsMeta(
        [FromQuery()] ContactFindManyArgs filter
    )
    {
        return Ok(await _service.ContactsMeta(filter));
    }

    /// <summary>
    /// Get one Contact
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<Contact>> Contact([FromRoute()] ContactWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Contact(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Contact
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateContact(
        [FromRoute()] ContactWhereUniqueInput uniqueId,
        [FromQuery()] ContactUpdateInput contactUpdateDto
    )
    {
        try
        {
            await _service.UpdateContact(uniqueId, contactUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a Customer record for Contact
    /// </summary>
    [HttpGet("{Id}/customer")]
    public async Task<ActionResult<List<Customer>>> GetCustomer(
        [FromRoute()] ContactWhereUniqueInput uniqueId
    )
    {
        var customer = await _service.GetCustomer(uniqueId);
        return Ok(customer);
    }
}
