using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.Infrastructure.Models;

namespace CrmBackendApiNet.APIs.Extensions;

public static class LeadsExtensions
{
    public static Lead ToDto(this LeadDbModel model)
    {
        return new Lead
        {
            CreatedAt = model.CreatedAt,
            Customer = model.CustomerId,
            Email = model.Email,
            FirstName = model.FirstName,
            Id = model.Id,
            LastName = model.LastName,
            Opportunities = model.Opportunities?.Select(x => x.Id).ToList(),
            Status = model.Status,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static LeadDbModel ToModel(this LeadUpdateInput updateDto, LeadWhereUniqueInput uniqueId)
    {
        var lead = new LeadDbModel
        {
            Id = uniqueId.Id,
            Email = updateDto.Email,
            FirstName = updateDto.FirstName,
            LastName = updateDto.LastName,
            Status = updateDto.Status
        };

        if (updateDto.CreatedAt != null)
        {
            lead.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Customer != null)
        {
            lead.CustomerId = updateDto.Customer;
        }
        if (updateDto.UpdatedAt != null)
        {
            lead.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return lead;
    }
}
