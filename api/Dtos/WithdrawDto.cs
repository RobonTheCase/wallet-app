using System.ComponentModel.DataAnnotations;

public class WithdrawDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public decimal Amount { get; set; }
}