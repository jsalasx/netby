
using MsTransactions.Domain.Entities;
using Shared.Dto.Transaction;

public static class TransactionMapper
{
    public static TransactionResponseDto ToDto(this TransactionEntity entity)
    {
        return new TransactionResponseDto
        {
            Id = entity.Id,
            Type = entity.Type,
            Details = entity.Details.Select(d => new TransactionDetailResponseDto
            {
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                ProductCode = d.ProductCode,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Total = d.Total
            }).ToList(),
            Comment = entity.Comment,
            TotalAmount = entity.TotalAmount,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static IEnumerable<TransactionResponseDto> ToDtoList(this IEnumerable<TransactionEntity> entities)
    {
        return entities.Select(e => e.ToDto());
    }

    public static TransactionEntity ToEntity(this CreateTransactionRequestDto dto)
    {
        return new TransactionEntity
        {
            Id = Guid.NewGuid(),
            Type = dto.Type ?? throw new ArgumentNullException(nameof(dto.Type)),
            Details = dto.Details.Select(d => new TransactionDetailEntity
            {
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                ProductCode = d.ProductCode,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Total = d.Total
            }).ToList(),
            Comment = dto.Comment,
            TotalAmount = dto.TotalAmount,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    // public static void UpdateEntity(UpdateTransactionRequestDto dto, Guid id)
    // {
    //     var entity = new TransactionEntity { Id = id };
    //     entity.Type = dto.Type ?? entity.Type;
    //     entity.Details = dto.Details.Select(d => new TransactionDetailEntity
    //     {
    //         ProductId = d.ProductId,
    //         Quantity = d.Quantity,
    //         UnitPrice = d.UnitPrice,
    //         Total = d.Total
    //     }).ToList();
    //     entity.Comment = dto.Comment;
    //     entity.TotalAmount = dto.TotalAmount;
    //     entity.UpdatedAt = DateTime.UtcNow;
    // }
}