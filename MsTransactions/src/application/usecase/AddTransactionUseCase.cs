using MsTransactions.Domain.Entities;
using MsTransactions.Domain.Port;
using MsTransactions.Infrastructure.Adapters;
using MsTransactions.Infrastructure.Persistence;
using Shared.Dto.Transaction;

namespace MsTransactions.Application.UseCase;

public class AddTransactionUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ProductApiClient _productApiClient;
    private int totalDetailMultiplyTotalAmount;
    private int totalSumDetailAmount;
    private int requestTotalAmount;

    private readonly AppDbContext _db;

    public AddTransactionUseCase(ITransactionRepository transactionRepository, ProductApiClient productApiClient, AppDbContext db)
    {
        _transactionRepository = transactionRepository;
        _productApiClient = productApiClient;
        _db = db;
    }

    public async Task<TransactionResponseDto> ExecuteAsync(TransactionEntity transaction, CancellationToken ct = default)
    {
        // Lógica para agregar una nueva transacción
        try
        {
            var stockResp = await _productApiClient.UpdateStockAsync(transaction.GetUpdateStockDtoRequest(), ct);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating stock in Product Service: " + ex.Message);
        }
        await using var dbTransaction = await _db.Database.BeginTransactionAsync(ct);
        try
        {

            transaction.Id = Guid.NewGuid();
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.UpdatedAt = DateTime.UtcNow;


            totalDetailMultiplyTotalAmount = transaction.GetDetailMultiplyTotalAmount();
            totalSumDetailAmount = transaction.CalculateTotalAmount();
            requestTotalAmount = transaction.TotalAmount;

            var totals = new List<int> { totalDetailMultiplyTotalAmount, totalSumDetailAmount, requestTotalAmount };

            int max = totals.Max();
            int min = totals.Min();
            bool isValid = (max - min) <= 100;
            if (!isValid)
            {
                throw new Exception("The total amount is not valid");
            }


            await _transactionRepository.AddAsync(transaction, ct);
            await dbTransaction.CommitAsync(ct);
            return TransactionMapper.ToDto(transaction);
        }
        catch (Exception ex)
        {
            await dbTransaction.RollbackAsync(ct);
            transaction.ChangeTypeByError();
            await _productApiClient.UpdateStockAsync(transaction.GetUpdateStockDtoRequest(), ct);
            throw new Exception("Error adding transaction: " + ex.Message);
        }
    }
}