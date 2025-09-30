using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmBackendApiNet.Infrastructure.Models;

[Table("Leads")]
public class LeadDbModel
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    public string? CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public CustomerDbModel? Customer { get; set; } = null;

    public string? Email { get; set; }

    [StringLength(1000)]
    public string? FirstName { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    [StringLength(1000)]
    public string? LastName { get; set; }

    public List<OpportunityDbModel>? Opportunities { get; set; } = new List<OpportunityDbModel>();

    [StringLength(1000)]
    public string? Status { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
