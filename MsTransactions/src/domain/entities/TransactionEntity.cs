
using MsTransactions.Domain.Enums;

namespace MsTransactions.Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public TransactionTypeEnum Type { get; set; }

    public IEnumerable<TransactionDetail>? TransactionDetail { get; set; }

    public int TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}

