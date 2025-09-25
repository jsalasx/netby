namespace Shared.Dto.Transaction;



public class UpdateStockResponseDto
{
    public bool Success { get; set; }
    public List<UpdateStockErrorResponseDto> Errors { get; set; } = new List<UpdateStockErrorResponseDto>();

    public string Message { get; set; } = string.Empty;

}

public class UpdateStockErrorResponseDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
