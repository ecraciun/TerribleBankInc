using System.ComponentModel.DataAnnotations;

namespace TerribleBankInc.Models.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}
