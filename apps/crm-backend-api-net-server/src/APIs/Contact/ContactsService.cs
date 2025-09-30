using CrmBackendApiNet.Infrastructure;

namespace CrmBackendApiNet.APIs;

public class ContactsService : ContactsServiceBase
{
    public ContactsService(CrmBackendApiNetDbContext context)
        : base(context) { }
}
