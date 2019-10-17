using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services
{
    public class BankAccountService : IBankAccountService
    {
        public Task<BankAccount> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BankAccount> GetByAccountNumber(string accountNumber)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestNewBankAccount(BankAccount newAccount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApproveAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DenyAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BlockAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EnableAccount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBalance(int id, decimal delta)
        {
            throw new NotImplementedException();
        }
    }
}
