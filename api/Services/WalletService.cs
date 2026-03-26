using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Services;

public class WalletService
{
    private readonly AppDbContext _context;

    public WalletService(AppDbContext context)
    {
        _context = context;
    }

    public ApiResponse<object> Deposit(DepositDto request)
    {
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == request.UserId);

        if (wallet == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Wallet not found"
            };
        }

        wallet.Balance += request.Amount;

        var transaction = new Transaction
        {
            Amount = request.Amount,
            Type = "CREDIT",
            WalletId = wallet.Id
        };

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Deposit successful",
            Data = new { balance = wallet.Balance }
        };
    }

    public ApiResponse<object> Withdraw(WithdrawDto request)
    {
        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == request.UserId);

        if (wallet == null)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Wallet not found"
            };
        }

        if (wallet.Balance < request.Amount)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Insufficient funds"
            };
        }

        wallet.Balance -= request.Amount;

        var transaction = new Transaction
        {
            Amount = request.Amount,
            Type = "DEBIT",
            WalletId = wallet.Id
        };

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return new ApiResponse<object>
        {
            Success = true,
            Message = "Withdrawal successful",
            Data = new { balance = wallet.Balance }
        };
    }

    public ApiResponse<List<TransactionResponseDto>> GetTransactions(int userId)
    {
      var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);

      if (wallet == null)
      {
        return new ApiResponse<List<TransactionResponseDto>>
       {
          Success = false,
          Message = "Wallet not found"
        };
      }

    var transactions = _context.Transactions
        .Where(t => t.WalletId == wallet.Id)
        .OrderByDescending(t => t.CreatedAt)
        .Select(t => new TransactionResponseDto
        {
            Amount = t.Amount,
            Type = t.Type,
            CreatedAt = t.CreatedAt.ToString("dd MMM yyyy HH:mm") 
        })
        .ToList();

        return new ApiResponse<List<TransactionResponseDto>>
        {
          Success = true,
          Message = "Transactions retrieved",
          Data = transactions
        };
    }
}