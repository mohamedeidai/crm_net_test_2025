using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmBackendApiNet.Infrastructure.Models;

[Table("Opportunities")]
public class OpportunityDbModel
{
    [Range(-999999999, 999999999)]
    public double? Amount { get; set; }

    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? LeadId { get; set; }

    [ForeignKey(nameof(LeadId))]
    public LeadDbModel? Lead { get; set; } = null;

    [StringLength(1000)]
    public string? Stage { get; set; }

    [StringLength(1000)]
    public string? Title { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
