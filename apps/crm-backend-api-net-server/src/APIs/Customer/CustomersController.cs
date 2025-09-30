using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[ApiController()]
public class CustomersController : CustomersControllerBase
{
    public CustomersController(ICustomersService service)
        : base(service) { }
}
