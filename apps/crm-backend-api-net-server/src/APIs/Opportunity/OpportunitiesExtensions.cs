using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.Infrastructure.Models;

namespace CrmBackendApiNet.APIs.Extensions;

public static class OpportunitiesExtensions
{
    public static Opportunity ToDto(this OpportunityDbModel model)
    {
        return new Opportunity
        {
            Amount = model.Amount,
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Lead = model.LeadId,
            Stage = model.Stage,
            Title = model.Title,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static OpportunityDbModel ToModel(
        this OpportunityUpdateInput updateDto,
        OpportunityWhereUniqueInput uniqueId
    )
    {
        var opportunity = new OpportunityDbModel
        {
            Id = uniqueId.Id,
            Amount = updateDto.Amount,
            Stage = updateDto.Stage,
            Title = updateDto.Title
        };

        if (updateDto.CreatedAt != null)
        {
            opportunity.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Lead != null)
        {
            opportunity.LeadId = updateDto.Lead;
        }
        if (updateDto.UpdatedAt != null)
        {
            opportunity.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return opportunity;
    }
}
