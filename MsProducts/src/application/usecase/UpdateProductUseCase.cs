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

    public async Task Execute(ProductEntity product, CancellationToken ct = default)
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

        // Verificar que el producto existe
        var existingProduct = await _productRepository.GetByIdAsync(product.Id, ct);
        if (existingProduct == null)
            throw new InvalidOperationException($"Product with ID {product.Id} not found");

        // Actualizar la fecha de modificación
        product.UpdatedAt = DateTime.UtcNow;
        
        // Mantener la fecha de creación original
        product.CreatedAt = existingProduct.CreatedAt;

        await _productRepository.UpdateAsync(product, ct);
    }
}