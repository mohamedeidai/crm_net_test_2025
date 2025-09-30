namespace CrmBackendApiNet.APIs.Dtos;

public class LeadCreateInput
{
    public DateTime CreatedAt { get; set; }

    public Customer? Customer { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? Id { get; set; }

    public string? LastName { get; set; }

    public List<Opportunity>? Opportunities { get; set; }

    public string? Status { get; set; }

    public DateTime UpdatedAt { get; set; }
}
