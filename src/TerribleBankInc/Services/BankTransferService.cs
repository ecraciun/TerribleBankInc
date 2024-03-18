using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Repositories.Interfaces;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Services;

public class BankTransferService : IBankTransferService
{
    private readonly IMapper _mapper;
    private readonly IBankAccountService _bankAccountService;
    private readonly IBaseRepository<BankTransaction> _bankTransactionRepository;

    public BankTransferService(
        IBankAccountService bankAccountService,
        IMapper mapper,
        IBaseRepository<BankTransaction> bankTransactionRepository
    )
    {
        _mapper = mapper;
        _bankAccountService = bankAccountService;
        _bankTransactionRepository = bankTransactionRepository;
    }

    public async Task<bool> Send(BankTransactionViewModel transactionViewModel)
    {
        if (transactionViewModel.Amount <= 0m)
            return false;

        BankAccount sourceAccount = await _bankAccountService.GetByAccountNumber(
            transactionViewModel.SourceAccountNumber
        );
        if (sourceAccount == null)
            return false;

        BankAccount destinationAccount = await _bankAccountService.GetByAccountNumber(
            transactionViewModel.DestinationAccountNumber
        );
        if (destinationAccount == null)
            return false;

        if (
            sourceAccount.ClientId != transactionViewModel.SourceClientId
            || sourceAccount.Currency != transactionViewModel.Currency
            || destinationAccount.Currency != transactionViewModel.Currency
            || sourceAccount.Balance < transactionViewModel.Amount
        )
            return false;

        BankTransaction transaction = _mapper.Map<BankTransaction>(transactionViewModel);
        transaction.Timestamp = DateTime.UtcNow;
        transaction.Approved = true;
        transaction.DestinationAccountId = destinationAccount.ID;
        transaction.DestinationClientId = destinationAccount.Client.ID;
        transaction.SourceAccountId = sourceAccount.ID;

        await _bankTransactionRepository.AddAsync(transaction);
        await _bankAccountService.UpdateBalance(sourceAccount.ID, -transactionViewModel.Amount);
        await _bankAccountService.UpdateBalance(destinationAccount.ID, transactionViewModel.Amount);

        return true;
    }

    public async Task<List<BankTransaction>> GetAllForAccount(int accountId)
    {
        return await _bankTransactionRepository.Get(x =>
            x.SourceAccountId == accountId || x.DestinationAccountId == accountId
        );
    }

    public async Task<List<BankTransaction>> GetAllForClient(int clientId)
    {
        return await _bankTransactionRepository.Get(x =>
            x.SourceClientId == clientId || x.DestinationClientId == clientId
        );
    }
}
