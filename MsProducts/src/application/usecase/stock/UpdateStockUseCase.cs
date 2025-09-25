using MsProducts.Domain.Ports;
using Shared.Dto.Transaction;
using RedLockNet.SERedis;
using MsProducts.Infrastructure.Persistence;
using RedLockNet;

namespace MsProducts.Application.UseCase;

public class UpdateStockUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly RedLockFactory _redLockFactory;
    private readonly AppDbContext _db;

    public UpdateStockUseCase(IProductRepository productRepository, RedLockFactory redLockFactory, AppDbContext db)
    {
        _productRepository = productRepository;
        _redLockFactory = redLockFactory;
        _db = db;
    }

    public async Task<UpdateStockResponseDto> Execute(List<UpdateStockDtoRequest> request, CancellationToken ct = default)
    {
        var response = new UpdateStockResponseDto();
        var locks = new List<IRedLock>();
        await using var dbTransaction = await _db.Database.BeginTransactionAsync(ct);
        var hasError = false;
        try
        {
            foreach (var item in request)
            {
                // Lock por producto
                var resource = $"lock:product:{item.ProductId}";
                var redLock = await _redLockFactory.CreateLockAsync(
                    resource,
                    expiryTime: TimeSpan.FromSeconds(30),
                    waitTime: TimeSpan.FromSeconds(10),
                    retryTime: TimeSpan.FromMilliseconds(200));

                if (!redLock.IsAcquired)
                {
                    response.Errors.Add(new UpdateStockErrorResponseDto
                    {
                        ProductId = item.ProductId,
                        ProductName = "Unknown",
                        ErrorMessage = "No se pudo adquirir lock"
                    });
                    hasError = true;
                }

                locks.Add(redLock);

                // 🚀 Actualizar stock
                var repoResponse = await _productRepository.UpdateStockAsync(item.ProductId, item.Quantity, ct);

                if (!repoResponse.Success)
                {
                    response.Errors.Add(new UpdateStockErrorResponseDto
                    {
                        ProductId = item.ProductId,
                        ProductName = repoResponse.ProductName ?? "Unknown",
                        ErrorMessage = repoResponse.Message ?? "Error en stock"
                    });
                    hasError = true;
                }
            }
            if (hasError)
            { 
                throw new Exception("Errores en actualización de stock");
            }

            // Si todos pasaron → confirmamos DB
            await dbTransaction.CommitAsync(ct);

            response.Success = true;
            return response;
        }
        catch
        {
            // Si algo falla → rollback DB
            await dbTransaction.RollbackAsync(ct);
            response.Success = false;
            return response;
        }
        finally
        {
            // 🔓 Liberar todos los locks
            foreach (var redLock in locks)
                redLock.Dispose();
        }
    }
}
