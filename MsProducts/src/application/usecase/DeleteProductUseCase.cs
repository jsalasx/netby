using MsProducts.Domain.Ports;

namespace MsProducts.Application.UseCase;

public class DeleteProductUseCase
{
    private readonly IProductRepository _productRepository;

    public DeleteProductUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Execute(Guid id, CancellationToken ct = default)
    {
        // Validación básica
        if (id == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty", nameof(id));

        // Verificar que el producto existe antes de eliminarlo
        var existingProduct = await _productRepository.GetByIdAsync(id, ct);
        if (existingProduct == null)
            throw new InvalidOperationException($"Product with ID {id} not found");

        await _productRepository.DeleteAsync(id, ct);
    }
}