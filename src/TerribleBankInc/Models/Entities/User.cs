namespace TerribleBankInc.Models.Entities
{
    public class User : BaseEntity
    {
        public int ClientId { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public bool IsAdmin { get; set; }

        public Client Client { get; set; }
    }
}