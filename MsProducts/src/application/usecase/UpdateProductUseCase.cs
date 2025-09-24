using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;

namespace MsProducts.Application.UseCase;

public class UpdateProductUseCase
{
    private readonly IProductRepository _productRepository;

    public UpdateProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductEntity?> Execute(ProductEntity product, CancellationToken ct = default)
    {
        // Validaciones de negocio
        if (product.Id == Guid.Empty)
            throw new ArgumentException("Product ID is required for update", nameof(product.Id));

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required", nameof(product.Name));

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative", nameof(product.Price));

        if (product.Stock < 0)
            throw new ArgumentException("Product stock cannot be negative", nameof(product.Stock));

        // Actualizar la fecha de modificación
        product.UpdatedAt = DateTime.UtcNow;

        return await _productRepository.UpdateAsync(product, ct);
    }
}