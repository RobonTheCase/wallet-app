namespace WalletApi.Dtos;

public class AuthResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}