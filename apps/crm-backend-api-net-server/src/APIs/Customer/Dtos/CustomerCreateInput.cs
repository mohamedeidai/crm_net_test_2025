namespace CrmBackendApiNet.APIs.Dtos;

public class CustomerCreateInput
{
    public string? Address { get; set; }

    public List<Contact>? Contacts { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Email { get; set; }

    public string? Id { get; set; }

    public List<Lead>? Leads { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public DateTime UpdatedAt { get; set; }
}
