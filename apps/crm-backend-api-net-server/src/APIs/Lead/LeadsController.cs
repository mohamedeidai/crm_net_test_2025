using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[ApiController()]
public class LeadsController : LeadsControllerBase
{
    public LeadsController(ILeadsService service)
        : base(service) { }
}
