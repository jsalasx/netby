using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;

namespace MsProducts.Application.UseCase;

public class GetProductByIdUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductEntity?> Execute(Guid id, CancellationToken ct = default)
    {
        return await _productRepository.GetByIdAsync(id, ct);
    }
}