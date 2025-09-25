using Shared.Dto.Transaction;

namespace MsTransactions.Infrastructure.Adapters;
public class ProductApiClient
{
    private readonly HttpClient _http;

    public ProductApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<UpdateStockResponseDto?> UpdateStockAsync(
        List<UpdateStockDtoRequest> request,
        CancellationToken ct = default)
    {
        // POST JSON al endpoint
        var response = await _http.PostAsJsonAsync("api/stock/update/batch", request, ct);

        if (!response.IsSuccessStatusCode)
        {
            // manejar errores HTTP
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Error en API productos: {response.StatusCode} - {errorContent}");
        }

        // deserializar la respuesta
        return await response.Content.ReadFromJsonAsync<UpdateStockResponseDto>(cancellationToken: ct);
    }
}
