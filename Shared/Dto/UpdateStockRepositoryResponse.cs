namespace Shared.Dto;


public class UpdateStockRepositoryResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ProductName { get; set; }
}
