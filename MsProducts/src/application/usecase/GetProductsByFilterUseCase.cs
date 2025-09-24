using MsProducts.Application.Models;
using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;

namespace MsProducts.Application.UseCase;

public class GetProductsByFilterUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductsByFilterUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductEntity>> Execute(ProductFilterRequestDto filter, CancellationToken ct = default)
    {
        return await _productRepository.GetByFilterAsync(filter, ct);
    }
}