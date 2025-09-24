namespace MsProducts.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string ImageUri { get; set; } = null!;
    public int Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void DecrementStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (quantity > Stock)
            throw new InvalidOperationException("Insufficient stock available");
        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void IncrementStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}