using System.ComponentModel.DataAnnotations;

namespace TerribleBankInc.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}