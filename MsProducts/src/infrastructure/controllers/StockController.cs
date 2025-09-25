using Microsoft.AspNetCore.Mvc;
using MsProducts.Application.UseCase;
using Shared.Dto.Transaction;


namespace MsProducts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{


    private readonly UpdateStockUseCase _updateStockUseCase;

    public StockController(UpdateStockUseCase updateStockUseCase)
    {
        _updateStockUseCase = updateStockUseCase;
    }

    [HttpPost("update/batch")]
    public async Task<IActionResult> AdjustStock([FromBody] List<UpdateStockDtoRequest> requests, CancellationToken ct)
    {
        var result = await _updateStockUseCase.Execute(requests, ct);

        if (!result.Success)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(new { message = result.Message });
    }



}