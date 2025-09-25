using MsProducts.Domain.Entities;
using Shared.Dto;

namespace MsProducts.Infrastructure.Mappers;

public static class ProductMapper
{
    public static ProductEntity ToEntity(CreateProductRequestDto dto)
    {
        return new ProductEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Code = dto.Code,
            Category = dto.Category,
            ImageUri = dto.ImageUri,
            Price = dto.Price,
            Stock = dto.Stock,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static ProductEntity ToEntity(UpdateProductRequestDto dto, Guid id)
    {
        return new ProductEntity
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            ImageUri = dto.ImageUri,
            Price = dto.Price,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static ProductResponseDto ToResponse(ProductEntity entity)
    {
        return new ProductResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Description = entity.Description,
            Category = entity.Category,
            ImageUri = entity.ImageUri,
            Price = entity.Price,
            Stock = entity.Stock,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
