using MsProducts.Application.Models;
using MsProducts.Domain.Entities;
using MsProducts.Domain.Ports;
using MsProducts.Infrastructure.Mappers;

namespace MsProducts.Application.UseCase;

public class GetProductsByFilterUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductsByFilterUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductFilterResponseDto> Execute(ProductFilterRequestDto filter, CancellationToken ct = default)
    {
        var products = await _productRepository.GetByFilterAsync(filter, ct);
        var count = await _productRepository.CountByFilterAsync(filter, ct);
        return new ProductFilterResponseDto
        {
            Products = products.Select(ProductMapper.ToResponse),
            TotalCount = count,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }
}