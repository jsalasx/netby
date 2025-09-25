namespace Shared.Dto;

public class CreateProductRequestDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ImageUri { get; set; } = null!;
    public int Price { get; set; }
    public int Stock { get; set; }
}

public class UpdateProductRequestDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ImageUri { get; set; } = null!;
    public int Price { get; set; }
}

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ImageUri { get; set; } = null!;
    public int Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}