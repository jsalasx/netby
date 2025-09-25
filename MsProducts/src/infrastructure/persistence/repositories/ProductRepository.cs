using Microsoft.EntityFrameworkCore;
using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;
using Shared.Dto;

namespace MsProducts.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db) => _db = db;

    public async Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Products.FindAsync(new object[] { id }, ct);
    }

    public async Task<long> CountByFilterAsync(ProductFilterRequestDto filter, CancellationToken ct)
    {
        var query = _db.Products.AsQueryable();

        if (filter.Id.HasValue)
            query = query.Where(p => p.Id == filter.Id.Value);

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(p => p.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Category))
            query = query.Where(p => p.Category == filter.Category);

        if (filter.PriceGreaterThanEqual.HasValue)
            query = query.Where(p => p.Price >= filter.PriceGreaterThanEqual.Value);

        if (filter.PriceLessThan.HasValue)
            query = query.Where(p => p.Price < filter.PriceLessThan.Value);

        if (filter.StockGreaterThanEqual.HasValue)
            query = query.Where(p => p.Stock >= filter.StockGreaterThanEqual.Value);

        if (filter.StockLessThan.HasValue)
            query = query.Where(p => p.Stock < filter.StockLessThan.Value);

        return await query.LongCountAsync(ct);
    }

    public async Task<IEnumerable<ProductEntity>> GetByFilterAsync(ProductFilterRequestDto filter, CancellationToken ct)
    {
        var query = _db.Products.AsQueryable();

        if (filter.Id.HasValue)
            query = query.Where(p => p.Id == filter.Id.Value);

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(p => p.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Category))
            query = query.Where(p => p.Category == filter.Category);

        if (filter.PriceGreaterThanEqual.HasValue)
            query = query.Where(p => p.Price >= filter.PriceGreaterThanEqual.Value);

        if (filter.PriceLessThan.HasValue)
            query = query.Where(p => p.Price < filter.PriceLessThan.Value);

        if (filter.StockGreaterThanEqual.HasValue)
            query = query.Where(p => p.Stock >= filter.StockGreaterThanEqual.Value);

        if (filter.StockLessThan.HasValue)
            query = query.Where(p => p.Stock < filter.StockLessThan.Value);

        // Aplicar paginación
        int skip = (filter.Page - 1) * filter.PageSize;
        query = query.Skip(skip).Take(filter.PageSize);

        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(ProductEntity product, CancellationToken ct)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<ProductEntity?> UpdateAsync(ProductEntity product, CancellationToken ct)
    {
        var existing = await _db.Products.FirstOrDefaultAsync(p => p.Id == product.Id, ct);
        if (existing is null)
            throw new InvalidOperationException($"Product with Id {product.Id} not found");

        // Actualizar propiedades
        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Category = product.Category;
        existing.ImageUri = product.ImageUri;
        existing.Price = product.Price;
        existing.Stock = product.Stock;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var product = await _db.Products.FindAsync(new object[] { id }, ct);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync(ct);
        }
    }

    public async Task<UpdateStockRepositoryResponse> UpdateStockAsync(Guid productId, int quantity, CancellationToken ct)
{
    var product = await _db.Products.FindAsync(new object[] { productId }, ct);

    if (product == null)
        return new UpdateStockRepositoryResponse { Success = false, ProductName= "", Message = "Product not found" };

    product.Stock += quantity;

    if (product.Stock < 0)
        return new UpdateStockRepositoryResponse { Success = false, ProductName= product.Name, Message = "Insufficient stock" };

    await _db.SaveChangesAsync(ct);

    return new UpdateStockRepositoryResponse { Success = true, ProductName= product.Name, Message = "Stock updated successfully" };
}
}
