using Microsoft.EntityFrameworkCore;
using MsProducts.Application.Models;
using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;

namespace MsProducts.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db) => _db = db;

    public async Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Products.FindAsync(new object[] { id }, ct);
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

    public async Task UpdateAsync(ProductEntity product, CancellationToken ct)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync(ct);
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
}
