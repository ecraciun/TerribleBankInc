using System.Collections.Generic;
using TerribleBankInc.Models.Enums;

namespace TerribleBankInc.Models.Entities
{
    public class BankAccount : BaseEntity
    {
        public CurrencyTypes Currency { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }
        public bool Enabled { get; set; }
        public bool? Approved { get; set; }
        public string Reason { get; set; }

        public List<BankTransaction> IncomingTransactions { get; set; }
        public List<BankTransaction> OutgoingTransactions { get; set; }
        public Client Client { get; set; }
    }
}