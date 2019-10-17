namespace TerribleBankInc.Models.Dtos
{
    public class ClientUser
    {
        public bool IsAdmin { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}