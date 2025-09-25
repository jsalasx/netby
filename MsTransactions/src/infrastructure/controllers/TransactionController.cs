using Microsoft.AspNetCore.Mvc;
using MsTransactions.Application.UseCase;
using Shared.Dto.Transaction;


namespace MsTransactions.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{


    private readonly AddTransactionUseCase _addTransactionUseCase;
    private readonly DeleteTransactionUseCase _deleteTransactionUseCase;
    private readonly GetTransactionByFilterUseCase _getTransactionByFilterUseCase;
    private readonly GetTransactionByIdUseCase _getTransactionByIdUseCase;

    public TransactionController(AddTransactionUseCase addTransactionUseCase,
        DeleteTransactionUseCase deleteTransactionUseCase,
        GetTransactionByFilterUseCase getTransactionByFilterUseCase,
        GetTransactionByIdUseCase getTransactionByIdUseCase)
    {
        _addTransactionUseCase = addTransactionUseCase;
        _deleteTransactionUseCase = deleteTransactionUseCase;
        _getTransactionByFilterUseCase = getTransactionByFilterUseCase;
        _getTransactionByIdUseCase = getTransactionByIdUseCase;
    }

    // 🔹 GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getTransactionByIdUseCase.ExecuteAsync(id, ct);
        return Ok(result);

    }

    // 🔹 GET BY FILTER
    [HttpPost("filter")]
    public async Task<IActionResult> GetByFilter([FromBody] TransactionFilterRequestDto request, CancellationToken ct)
    {
        var result = await _getTransactionByFilterUseCase.ExecuteAsync(request, ct);
        return Ok(result);
    }

    // 🔹 CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequestDto request, CancellationToken ct)
    {
        var entity = TransactionMapper.ToEntity(request);
        var result = await _addTransactionUseCase.ExecuteAsync(entity, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    // 🔹 DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _deleteTransactionUseCase.ExecuteAsync(id, ct);
        return NoContent();
    }


}