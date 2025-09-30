using CrmBackendApiNet.Infrastructure;

namespace CrmBackendApiNet.APIs;

public class CustomersService : CustomersServiceBase
{
    public CustomersService(CrmBackendApiNetDbContext context)
        : base(context) { }
}
