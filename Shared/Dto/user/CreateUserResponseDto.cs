namespace Shared.Dto.User;

public class CreateUserResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
    public Guid UserId { get; set; }
}