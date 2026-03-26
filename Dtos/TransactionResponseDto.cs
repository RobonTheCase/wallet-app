namespace WalletApi.Dtos;

public class TransactionResponseDto
{
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}