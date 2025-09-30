using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[ApiController()]
public class ContactsController : ContactsControllerBase
{
    public ContactsController(IContactsService service)
        : base(service) { }
}
