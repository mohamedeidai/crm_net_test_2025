using CrmBackendApiNet.APIs.Dtos;
using CrmBackendApiNet.Infrastructure.Models;

namespace CrmBackendApiNet.APIs.Extensions;

public static class ContactsExtensions
{
    public static Contact ToDto(this ContactDbModel model)
    {
        return new Contact
        {
            CreatedAt = model.CreatedAt,
            Customer = model.CustomerId,
            Email = model.Email,
            FirstName = model.FirstName,
            Id = model.Id,
            LastName = model.LastName,
            Phone = model.Phone,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ContactDbModel ToModel(
        this ContactUpdateInput updateDto,
        ContactWhereUniqueInput uniqueId
    )
    {
        var contact = new ContactDbModel
        {
            Id = uniqueId.Id,
            Email = updateDto.Email,
            FirstName = updateDto.FirstName,
            LastName = updateDto.LastName,
            Phone = updateDto.Phone
        };

        if (updateDto.CreatedAt != null)
        {
            contact.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Customer != null)
        {
            contact.CustomerId = updateDto.Customer;
        }
        if (updateDto.UpdatedAt != null)
        {
            contact.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return contact;
    }
}
