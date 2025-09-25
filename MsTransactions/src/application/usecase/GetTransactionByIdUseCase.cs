using MsTransactions.Domain.Port;
using Shared.Dto.Transaction;

namespace MsTransactions.Application.UseCase;
public class GetTransactionByIdUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    public GetTransactionByIdUseCase(ITransactionRepository transactionRepository)
    {   
        _transactionRepository = transactionRepository;
        
    }

    public async Task<TransactionResponseDto> ExecuteAsync(Guid transactionId, CancellationToken ct= default)
    {
        // Lógica para obtener una transacción por su ID
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, ct);
        if (transaction == null)
        {
            throw new Exception("Transaction not found");
        }
        return TransactionMapper.ToDto(transaction);
    }
}