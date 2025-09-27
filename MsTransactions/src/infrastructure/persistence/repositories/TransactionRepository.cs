namespace MsTransactions.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using MsTransactions.Domain.Entities;
using Shared.Dto.Transaction;
using MsTransactions.Domain.Port;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TransactionEntity transaction, CancellationToken ct)
    {
        await _context.Transactions.AddAsync(transaction, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<long> CountByFilterAsync(TransactionFilterRequestDto filter, CancellationToken ct)
    {
        var query = ApplyFilter(_context.Transactions.AsQueryable(), filter, includeDetails: false);
        return await query.LongCountAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var transaction = await _context.Transactions.FindAsync(new object[] { id }, ct);
        if (transaction != null)
        {
            transaction.IsDeleted = true;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<IEnumerable<TransactionEntity>> GetByFilterAsync(TransactionFilterRequestDto filter, CancellationToken ct)
    {
        var query = ApplyFilter(_context.Transactions.AsQueryable(), filter, includeDetails: true);
        int skip = (filter.Page - 1) * filter.PageSize;
        query = query.Skip(skip).Take(filter.PageSize);
        return await query.ToListAsync(ct);

    }

    public async Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Transactions
            .Include(t => t.Details)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<TransactionEntity?> UpdateAsync(TransactionEntity transaction, CancellationToken ct)
    {
        var existingTransaction = await _context.Transactions
            .Include(t => t.Details)
            .FirstOrDefaultAsync(t => t.Id == transaction.Id, ct);

        if (existingTransaction is null)
            throw new KeyNotFoundException($"Transaction with Id {transaction.Id} not found.");

        existingTransaction.Type = transaction.Type;
        existingTransaction.TotalAmount = transaction.TotalAmount;
        existingTransaction.UpdatedAt = DateTime.UtcNow;
        existingTransaction.Details = transaction.Details;


        await _context.SaveChangesAsync(ct);
        return existingTransaction;
    }

    private IQueryable<TransactionEntity> ApplyFilter(IQueryable<TransactionEntity> query, TransactionFilterRequestDto filter, bool includeDetails)
    {
        Console.WriteLine("Applying filter...");
        Console.WriteLine($"Filter: {System.Text.Json.JsonSerializer.Serialize(filter)}");
        if (includeDetails)
            query = query.Include(t => t.Details);

        if (filter.Id.HasValue)
            query = query.Where(t => t.Id == filter.Id.Value);

        if (filter.Type.HasValue)
            query = query.Where(t => t.Type == filter.Type.Value);

        if (filter.ProductIds != null && filter.ProductIds.Any())
            query = query.Where(t => t.Details.Any(d => filter.ProductIds.Contains(d.ProductId)));

        if (filter.TotalAmountGreaterThanEqual.HasValue)
            query = query.Where(t => t.TotalAmount >= filter.TotalAmountGreaterThanEqual.Value);

        if (filter.TotalAmountLessThan.HasValue)
            query = query.Where(t => t.TotalAmount < filter.TotalAmountLessThan.Value);

        if (filter.CreatedAfterEqual.HasValue)
            query = query.Where(t => t.CreatedAt >= filter.CreatedAfterEqual.Value);

        if (filter.CreatedBefore.HasValue)
            query = query.Where(t => t.CreatedAt < filter.CreatedBefore.Value);

        query = query.OrderByDescending(t => t.CreatedAt);

        return query;
    }
}