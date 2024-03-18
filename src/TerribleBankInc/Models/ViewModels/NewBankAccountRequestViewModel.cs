using System.ComponentModel.DataAnnotations;
using TerribleBankInc.Models.Enums;

namespace TerribleBankInc.Models.ViewModels;

public class NewBankAccountRequestViewModel
{
    [Required]
    public CurrencyTypes Currency { get; set; }

    [Required]
    public int ClientId { get; set; }
}
