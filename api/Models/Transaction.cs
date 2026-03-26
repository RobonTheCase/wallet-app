namespace WalletApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // "deposit" or "withdraw"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int WalletId { get; set; }
    public Wallet Wallet { get; set; } = null!;
}