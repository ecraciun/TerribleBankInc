using System;
using TerribleBankInc.Models.Enums;

namespace TerribleBankInc.Models.Entities
{
    public class BankTransaction : BaseEntity
    {
        public int SourceClientId { get; set; }
        public int DestinationClientId { get; set; }
        public string DestinationClientEmail { get; set; }
        public string DestinationAccountNumber { get; set; }
        public int SourceAccountId { get; set; }
        public int? DestinationAccountId { get; set; }
        public bool Approved { get; set; }
        public string Reason { get; set; }
        public CurrencyTypes Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }

        public Client SourceClient { get; set; }
        public Client DestinationClient { get; set; }
        public BankAccount SourceBankAccount { get; set; }
        public BankAccount DestinationBankAccount { get; set; }
    }
}