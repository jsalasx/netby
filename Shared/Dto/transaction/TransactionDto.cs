namespace Shared.Dto.Transaction;

using Shared.Enums;

public class CreateTransactionRequestDto
{
    public TransactionTypeEnum? Type { get; set; }
    public List<TransactionDetailRequestDto> Details { get; set; } = new List<TransactionDetailRequestDto>();

    public int TotalAmount { get; set; }

    public string Coment { get; set; } = string.Empty;
}

public class TransactionDetailRequestDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    public int Total { get; set; }
}

public class UpdateTransactionRequestDto
{
    public Guid Id { get; set; }
    public TransactionTypeEnum? Type { get; set; }
    public List<TransactionDetailRequestDto> Details { get; set; } = new List<TransactionDetailRequestDto>();

    public string Coment { get; set; } = string.Empty;
    public int TotalAmount { get; set; }

}

public class TransactionResponseDto
{
    public Guid Id { get; set; }
    public TransactionTypeEnum Type { get; set; }
    public List<TransactionDetailResponseDto> Details { get; set; } = new List<TransactionDetailResponseDto>();
    public int TotalAmount { get; set; }

    public string Coment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


public class TransactionDetailResponseDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    public int Total { get; set; }
}