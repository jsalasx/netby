using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;

namespace MsProducts.Application.UseCase;

public class AddProductUseCase
{
    private readonly IProductRepository _productRepository;

    public AddProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Execute(ProductEntity product, CancellationToken ct = default)
    {
        // Validaciones de negocio podrían ir aquí
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required", nameof(product.Name));

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative", nameof(product.Price));

        if (product.Stock < 0)
            throw new ArgumentException("Product stock cannot be negative", nameof(product.Stock));

        // Asegurar que se establezcan las fechas
        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();

        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.AddAsync(product, ct);
    }
}