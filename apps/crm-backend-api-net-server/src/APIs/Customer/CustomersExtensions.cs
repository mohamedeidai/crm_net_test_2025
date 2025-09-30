using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.Infrastructure.Models;

namespace CrmBackendApiNet.APIs.Extensions;

public static class CustomersExtensions
{
    public static Customer ToDto(this CustomerDbModel model)
    {
        return new Customer
        {
            Address = model.Address,
            Contacts = model.Contacts?.Select(x => x.Id).ToList(),
            CreatedAt = model.CreatedAt,
            Email = model.Email,
            Id = model.Id,
            Leads = model.Leads?.Select(x => x.Id).ToList(),
            Name = model.Name,
            Phone = model.Phone,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static CustomerDbModel ToModel(
        this CustomerUpdateInput updateDto,
        CustomerWhereUniqueInput uniqueId
    )
    {
        var customer = new CustomerDbModel
        {
            Id = uniqueId.Id,
            Address = updateDto.Address,
            Email = updateDto.Email,
            Name = updateDto.Name,
            Phone = updateDto.Phone
        };

        if (updateDto.CreatedAt != null)
        {
            customer.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            customer.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return customer;
    }
}
