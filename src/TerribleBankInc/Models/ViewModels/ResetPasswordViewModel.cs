using System.ComponentModel.DataAnnotations;

namespace TerribleBankInc.Models.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public string Token { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Required]
    public string ConfirmPassword { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}
