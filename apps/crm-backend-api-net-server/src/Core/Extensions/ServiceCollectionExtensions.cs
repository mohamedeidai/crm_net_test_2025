using CrmBackendApiNet.APIs;

namespace CrmBackendApiNet;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IContactsService, ContactsService>();
        services.AddScoped<ICustomersService, CustomersService>();
        services.AddScoped<ILeadsService, LeadsService>();
        services.AddScoped<IOpportunitiesService, OpportunitiesService>();
    }
}
