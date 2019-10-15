using System.Collections.Generic;

namespace TerribleBankInc.Models
{
    public class BankAccount : BaseEntity
    {
        public CurrencyTypes Currency { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }

        public Client Client { get; set; }
        public List<BankTransaction> IncomingTransactions { get; set; }
        public List<BankTransaction> OutgoingTransactions { get; set; }
    }
}