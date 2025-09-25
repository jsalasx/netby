using MsTransactions.Domain.Port;
using MsTransactions.Infrastructure.Adapters;
using Shared.Enums;

namespace MsTransactions.Application.UseCase;
public class DeleteTransactionUseCase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ProductApiClient _productApiClient;
    private int totalDetailMultiplyTotalAmount;
    private int totalSumDetailAmount;
    private int requestTotalAmount;

    public DeleteTransactionUseCase(ITransactionRepository transactionRepository, ProductApiClient productApiClient)
    {
        _transactionRepository = transactionRepository;
        _productApiClient = productApiClient;
    }

    public async Task<bool> ExecuteAsync(Guid transactionId, CancellationToken ct= default)
    {
        // Logica para eliminar una transacción
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, ct);
        if (transaction == null)
        {
            throw new Exception("Transaction not found");
        }
        // Lógica para eliminar una transacción

        if (transaction.Type == TransactionTypeEnum.Return || transaction.Type == TransactionTypeEnum.PurchaseReturn || 
            transaction.Type == TransactionTypeEnum.Purchase)
        {
            throw new Exception("Cannot delete a " + transaction.Type + " transaction");
        }

        if (transaction.Type == TransactionTypeEnum.Sale)
        {
            transaction.Type = TransactionTypeEnum.Return;
        }

        try
        {
            var stockResp = await _productApiClient.UpdateStockAsync(transaction.GetUpdateStockDtoRequest(), ct);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating stock in Product Service: " + ex.Message);
        }

        await _transactionRepository.DeleteAsync(transactionId, ct);

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
        return true;
    }
}