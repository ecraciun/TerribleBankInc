using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBaseRepository<BankAccount> _bankAccountRepository;
        private readonly IMapper _mapper;

        public BankAccountService(IBaseRepository<BankAccount> bankAccountRepository, IMapper mapper)
        {
            _bankAccountRepository = bankAccountRepository;
            _mapper = mapper;
        }

        public async Task<BankAccount> GetById(int id)
        {
            return (await _bankAccountRepository.Get(x => x.ID == id, $"{nameof(BankAccount.Client)}")).FirstOrDefault();
        }

        public async Task<List<BankAccount>> GetAllAccountsForClient(int clientId)
        {
            return await _bankAccountRepository.Get(x => x.ClientId == clientId);
        }

        public async Task<List<BankAccount>> GetDisabledAccounts()
        {
            return await _bankAccountRepository.Get(x =>
                x.Enabled == false && x.Approved.HasValue && x.Approved.Value, $"{nameof(BankAccount.Client)}");
        }

        public async Task<List<BankAccount>> GetPendingAccounts()
        {
            return await _bankAccountRepository.Get(x => x.Enabled == false && !x.Approved.HasValue, $"{nameof(BankAccount.Client)}");
        }

        public async Task<BankAccount> GetByAccountNumber(string accountNumber)
        {
            var result = await _bankAccountRepository.Get(x => x.AccountNumber == accountNumber, $"{nameof(BankAccount.Client)}");
            return result.FirstOrDefault();
        }

        public async Task<bool> RequestNewBankAccount(NewBankAccountRequestViewModel newAccount)
        {
            var account = _mapper.Map<BankAccount>(newAccount);
            account.AccountNumber = Guid.NewGuid().ToString();
            account.Approved = null;
            await _bankAccountRepository.AddAsync(account);
            return true;
        }

        public async Task<bool> ApproveAccount(int id)
        {
            var account = await GetById(id);
            if (account != null && account.Enabled == false && account.Approved.HasValue == false)
            {
                account.Enabled = true;
                account.Approved = true;
                account.Balance = 100m;

                await _bankAccountRepository.UpdateAsync(account);

                return true;
            }

            return false;
        }

        public async Task<bool> RejectAccount(int id, string reason)
        {
            var account = await GetById(id);
            if (account != null && account.Enabled == false && account.Approved.HasValue == false)
            {
                account.Approved = false;
                account.Reason = reason;

                await _bankAccountRepository.UpdateAsync(account);

                return true;
            }

            return false;
        }

        public async Task<bool> BlockAccount(int id)
        {
            var account = await GetById(id);
            if (account != null && account.Enabled)
            {
                account.Enabled = false;

                await _bankAccountRepository.UpdateAsync(account);

                return true;
            }

            return false;
        }

        public async Task<bool> EnableAccount(int id)
        {
            var account = await GetById(id);
            if (account != null && !account.Enabled)
            {
                account.Enabled = true;

                await _bankAccountRepository.UpdateAsync(account);

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateBalance(int id, decimal delta)
        {
            var account = await GetById(id);
            if (account != null)
            {
                account.Balance += delta;

                await _bankAccountRepository.UpdateAsync(account);

                return true;
            }

            return false;
        }
    }
}