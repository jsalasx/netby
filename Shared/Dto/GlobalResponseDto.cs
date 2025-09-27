namespace Shared.Dto;

public class GlobalResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public object? Data { get; set; }

    public GlobalResponseDto(bool success, string message, object? data = null)
    {
        Success = success;
        Message = message;
        Data = data;
    }
}