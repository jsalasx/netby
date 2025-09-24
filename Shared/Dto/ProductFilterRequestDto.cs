namespace Shared.Dto;

public class ProductFilterRequestDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? PriceGreaterThanEqual { get; set; }
    public int? PriceLessThan { get; set; }
    public int? StockGreaterThanEqual { get; set; }
    public int? StockLessThan { get; set; }

    // 🔹 Paginación
    public int Page { get; set; } = 1;      // Página actual
    public int PageSize { get; set; } = 10; // Tamaño por página
}
