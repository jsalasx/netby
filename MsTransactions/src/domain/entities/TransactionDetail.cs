namespace MsTransactions.Domain.Entities;
public class TransactionDetail
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int Total { get; set; }
}