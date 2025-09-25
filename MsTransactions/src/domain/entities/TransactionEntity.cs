using Shared.Dto.Transaction;
using Shared.Enums;

namespace MsTransactions.Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public TransactionTypeEnum Type { get; set; }
    // Relación uno-a-muchos con detalles
    public ICollection<TransactionDetailEntity> Details { get; set; } = new List<TransactionDetailEntity>();
    public int TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string Coment { get; set; } = string.Empty;


    public int GetDetailMultiplyTotalAmount()
    {
        return Details.Sum(d => d.UnitPrice * d.Quantity);
    }

    public int CalculateTotalAmount()
    {
        return Details.Sum(d => d.Total);
    }

    public List<UpdateStockDtoRequest> GetUpdateStockDtoRequest()
    {
        if (Type == TransactionTypeEnum.Sale)
        {
            return Details.Select(d => new UpdateStockDtoRequest
            {
                ProductId = d.ProductId,
                Quantity = -Math.Abs(d.Quantity)
            }).ToList();
        } else if (Type == TransactionTypeEnum.Purchase)
        {
            return Details.Select(d => new UpdateStockDtoRequest
            {
                ProductId = d.ProductId,
                Quantity = +Math.Abs(d.Quantity)
            }).ToList();
        }
        else if (Type == TransactionTypeEnum.Return)
        {
            return Details.Select(d => new UpdateStockDtoRequest
            {
                ProductId = d.ProductId,
                Quantity = +Math.Abs(d.Quantity)
            }).ToList();
        } else if (Type == TransactionTypeEnum.PurchaseReturn)
        {
            return Details.Select(d => new UpdateStockDtoRequest
            {
                ProductId = d.ProductId,
                Quantity = -Math.Abs(d.Quantity)
            }).ToList();
        }
        else if (Type == TransactionTypeEnum.Purchase_Adjustment)
        {
            return Details.Select(d => new UpdateStockDtoRequest
            {
                ProductId = d.ProductId,
                Quantity = +Math.Abs(d.Quantity)
            }).ToList();
        }
        else if (Type == TransactionTypeEnum.Sale_Adjustment)
        {
            return Details.Select(d => new UpdateStockDtoRequest
            {
                ProductId = d.ProductId,
                Quantity = -Math.Abs(d.Quantity)
            }).ToList();
        }
        else
        {
            throw new Exception("Transaction type not supported for stock update");
        }

    }

}

