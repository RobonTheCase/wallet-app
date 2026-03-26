using Microsoft.AspNetCore.Mvc;
using WalletApi.Services;

namespace WalletApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly WalletService _walletService;

    public WalletController(WalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpPost("deposit")]
    public IActionResult Deposit(DepositDto request)
    {
        var result = _walletService.Deposit(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("withdraw")]
    public IActionResult Withdraw(WithdrawDto request)
    {
        var result = _walletService.Withdraw(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("transactions/{userId}")]
    public IActionResult GetTransactions(int userId)
    {
        var result = _walletService.GetTransactions(userId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}