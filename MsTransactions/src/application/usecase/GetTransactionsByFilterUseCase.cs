using MsTransactions.Domain.Port;
using Shared.Dto.Transaction;

namespace MsTransactions.Application.UseCase;

public class GetTransactionByFilterUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    public GetTransactionByFilterUseCase(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;

    }

    public async Task<TransactionFilterResponseDto> ExecuteAsync(TransactionFilterRequestDto filter, CancellationToken ct = default)
    {
        // Lógica para obtener transacciones por filtro
        var transactions = await _transactionRepository.GetByFilterAsync(filter, ct);
        if (transactions == null || !transactions.Any())
        {
            throw new Exception("Transaction not found");
        }

        var totalCount = await _transactionRepository.CountByFilterAsync(filter, ct);

        return new TransactionFilterResponseDto
        {
            Transactions = transactions.ToDtoList(),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }
    
    
}