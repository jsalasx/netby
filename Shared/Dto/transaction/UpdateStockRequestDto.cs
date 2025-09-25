namespace Shared.Dto.Transaction;



public class UpdateStockDtoRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
