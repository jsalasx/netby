using MsTransactions.Domain.Entities;
using Shared.Dto.Transaction;

namespace MsTransactions.Domain.Port;

public interface ITransactionRepository
{
    Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TransactionEntity>> GetByFilterAsync(TransactionFilterRequestDto filter, CancellationToken ct);

    Task<long> CountByFilterAsync(TransactionFilterRequestDto filter, CancellationToken ct);
    Task AddAsync(TransactionEntity transaction, CancellationToken ct);
    Task<TransactionEntity?> UpdateAsync(TransactionEntity transaction, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
