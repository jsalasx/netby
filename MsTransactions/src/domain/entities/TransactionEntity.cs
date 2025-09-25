using Shared.Enums;

namespace MsTransactions.Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public TransactionTypeEnum Type { get; set; }
    // Relación uno-a-muchos con detalles
    public ICollection<TransactionDetailEntity> details { get; set; } = new List<TransactionDetailEntity>();
    public int TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}

