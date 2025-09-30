using CrmBackendApiNet.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CrmBackendApiNet.Infrastructure;

public class CrmBackendApiNetDbContext : DbContext
{
    public CrmBackendApiNetDbContext(DbContextOptions<CrmBackendApiNetDbContext> options)
        : base(options) { }

    public DbSet<CustomerDbModel> Customers { get; set; }

    public DbSet<LeadDbModel> Leads { get; set; }

    public DbSet<OpportunityDbModel> Opportunities { get; set; }

    public DbSet<ContactDbModel> Contacts { get; set; }
}
