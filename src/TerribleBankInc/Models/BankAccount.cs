namespace TerribleBankInc.Models
{
    public class BankAccount : BaseEntity
    {
        public CurrencyTypes Currency { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}