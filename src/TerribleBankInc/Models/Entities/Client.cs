using System;
using System.Collections.Generic;

namespace TerribleBankInc.Models.Entities
{
    public class Client : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User User { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
    }
}