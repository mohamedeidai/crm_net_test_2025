namespace CrmBackendApiNet.APIs.Dtos;

public class LeadWhereInput
{
    public DateTime? CreatedAt { get; set; }

    public string? Customer { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? Id { get; set; }

    public string? LastName { get; set; }

    public List<string>? Opportunities { get; set; }

    public string? Status { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
