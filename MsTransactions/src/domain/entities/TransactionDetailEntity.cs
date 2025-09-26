namespace MsTransactions.Domain.Entities;

public class TransactionDetailEntity
{

    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    public int Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    // Clave foránea a TransactionEntity
    public Guid TransactionId { get; set; }
    public TransactionEntity? Transaction { get; set; }
}