using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Models;

namespace WalletApi.Services;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public ApiResponse<AuthResponseDto> Register(RegisterDto request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "User already exists"
            };
        }

        var user = new User
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        var wallet = new Wallet
        {
            UserId = user.Id,
            Balance = 0
        };

        _context.Wallets.Add(wallet);
        _context.SaveChanges();

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "User registered successfully",
            Data = new AuthResponseDto
            {
              Id = user.Id,
              Email = user.Email,
              Balance = 0
            }
        };
    }

    public ApiResponse<AuthResponseDto> Login(LoginDto request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
           return new ApiResponse<AuthResponseDto>
          {
            Success = false,
            Message = "Invalid email or password"
          };
        }

        var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == user.Id);

        return new ApiResponse<AuthResponseDto>
        {
            Success = true,
            Message = "Login successful",
            Data = new AuthResponseDto
            {
              Id = user.Id,
              Email = user.Email,
              Balance = wallet?.Balance ?? 0
            }
        };
    }
}