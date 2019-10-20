using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TerribleBankInc.Models.Enums;

namespace TerribleBankInc.Models.ViewModels
{
    public class BankTransactionViewModel
    {
        [Required]
        public int SourceClientId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string DestinationClientEmail { get; set; }
        [Required]
        public string DestinationAccountNumber { get; set; }
        public string Details { get; set; }
        [Required]
        public string SourceAccountNumber { get; set; }
        [Required]
        public CurrencyTypes Currency { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public List<SelectListItem> ActiveAccounts { get; set; }
    }
}