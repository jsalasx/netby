using MsProducts.Application.Models;
using MsProducts.Domain.Entities;

namespace MsProducts.Domain.Ports;

public interface IProductRepository
{
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<ProductEntity>> GetByFilterAsync(ProductFilterRequestDto filter, CancellationToken ct);

    Task<long> CountByFilterAsync(ProductFilterRequestDto filter, CancellationToken ct);
    Task AddAsync(ProductEntity product, CancellationToken ct);
    Task<ProductEntity?> UpdateAsync(ProductEntity product, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
