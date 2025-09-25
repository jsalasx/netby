using Microsoft.AspNetCore.Mvc;
using MsProducts.Application.UseCase;
using MsProducts.Infrastructure.Mappers;
using Shared.Dto;


namespace MsProducts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AddProductUseCase _addProductUseCase;
    private readonly GetProductByIdUseCase _getProductByIdUseCase;
    private readonly GetProductsByFilterUseCase _getProductsByFilterUseCase;
    private readonly DeleteProductUseCase _deleteProductUseCase;

    private readonly UpdateProductUseCase _updateProductUseCase;

    public ProductsController(
        AddProductUseCase addProductUseCase,
        GetProductByIdUseCase getProductByIdUseCase,
        GetProductsByFilterUseCase getProductsByFilterUseCase,
        DeleteProductUseCase deleteProductUseCase,
        UpdateProductUseCase updateProductUseCase)
    {
        _addProductUseCase = addProductUseCase;
        _getProductByIdUseCase = getProductByIdUseCase;
        _getProductsByFilterUseCase = getProductsByFilterUseCase;
        _deleteProductUseCase = deleteProductUseCase;
        _updateProductUseCase = updateProductUseCase;
    }

    
    // 🔹 GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        try
        {
            var product = await _getProductByIdUseCase.Execute(id, ct);
            if (product is null)
                return NotFound($"Product with ID {id} not found");

            var response = ProductMapper.ToResponse(product);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 🔹 POST CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequestDto request, CancellationToken ct)
    {
        try
        {
            var product = ProductMapper.ToEntity(request);
            await _addProductUseCase.Execute(product, ct);
            
            var response = ProductMapper.ToResponse(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // 🔹 PUT UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequestDto request, CancellationToken ct)
    {
        try
        {
            var product = ProductMapper.ToEntity(request, id);
            await _updateProductUseCase.Execute(product, ct);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // 🔹 DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _deleteProductUseCase.Execute(id, ct);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // 🔹 POST FILTER + PAGINACIÓN
    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilter([FromBody] ProductFilterRequestDto filter, CancellationToken ct)
    {
        try
        {
            var response = await _getProductsByFilterUseCase.Execute(filter, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
}