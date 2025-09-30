namespace CrmBackendApiNet.APIs.Dtos;

public class ContactCreateInput
{
    public DateTime CreatedAt { get; set; }

    public Customer? Customer { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? Id { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public DateTime UpdatedAt { get; set; }
}
