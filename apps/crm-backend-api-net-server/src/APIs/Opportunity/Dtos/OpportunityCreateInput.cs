namespace CrmBackendApiNet.APIs.Dtos;

public class OpportunityCreateInput
{
    public double? Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public Lead? Lead { get; set; }

    public string? Stage { get; set; }

    public string? Title { get; set; }

    public DateTime UpdatedAt { get; set; }
}
