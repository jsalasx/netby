namespace Shared.Dto;

public class ProductFilterResponseDto
{ 
    public IEnumerable<ProductResponseDto> Products { get; set; } = [];
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

}