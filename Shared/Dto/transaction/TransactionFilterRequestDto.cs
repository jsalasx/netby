namespace Shared.Dto.Transaction;

using Shared.Enums;
public class TransactionFilterRequestDto
{
    public Guid? Id { get; set; }

    public TransactionTypeEnum? Type { get; set; }

    public List<Guid>? ProductIds { get; set; }

    public int? TotalAmountGreaterThanEqual { get; set; }
    public int? TotalAmountLessThan { get; set; }

    public DateTime? CreatedAfterEqual { get; set; }
    public DateTime? CreatedBefore { get; set; }

    // 🔹 Paginación
    public int Page { get; set; } = 1;      // Página actual
    public int PageSize { get; set; } = 10; // Tamaño por página
}
