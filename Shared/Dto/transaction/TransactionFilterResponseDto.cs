namespace Shared.Dto.Transaction;

public class TransactionFilterResponseDto
{ 
    public IEnumerable<TransactionResponseDto> Transactions { get; set; } = [];
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

}