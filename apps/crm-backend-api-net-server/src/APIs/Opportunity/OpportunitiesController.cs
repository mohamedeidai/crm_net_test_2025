using Microsoft.AspNetCore.Mvc;

namespace CrmBackendApiNet.APIs;

[ApiController()]
public class OpportunitiesController : OpportunitiesControllerBase
{
    public OpportunitiesController(IOpportunitiesService service)
        : base(service) { }
}
