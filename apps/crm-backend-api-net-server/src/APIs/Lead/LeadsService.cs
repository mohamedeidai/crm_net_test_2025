using CrmBackendApiNet.Infrastructure;

namespace CrmBackendApiNet.APIs;

public class LeadsService : LeadsServiceBase
{
    public LeadsService(CrmBackendApiNetDbContext context)
        : base(context) { }
}
