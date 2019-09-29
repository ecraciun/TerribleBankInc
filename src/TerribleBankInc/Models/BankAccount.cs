namespace TerribleBankInc.Models
{
    public class BankAccount
    {
        public CurrencyTypes Currency { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }
        
    }
}