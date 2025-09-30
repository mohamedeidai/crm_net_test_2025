using CrmBackendApiNet.Infrastructure;

namespace CrmBackendApiNet.APIs;

public class OpportunitiesService : OpportunitiesServiceBase
{
    public OpportunitiesService(CrmBackendApiNetDbContext context)
        : base(context) { }
}
