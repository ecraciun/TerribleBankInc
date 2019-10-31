using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.Enums;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Controllers
{
    [Authorize]
    public class TransferController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IBankTransferService _bankTransferService;
        private readonly IBankAccountService _bankAccountService;

        public TransferController(IMapper mapper, IBankTransferService bankTransferService, IBankAccountService bankAccountService)
        {
            _mapper = mapper;
            _bankTransferService = bankTransferService;
            _bankAccountService = bankAccountService;
        }

        [HttpGet("/[controller]/[action]/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var transactions = await _bankTransferService.GetAllForAccount(accountId);

            return View(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var clientId = GetCurrentClientId();
            var transactions = await _bankTransferService.GetAllForClient(clientId);
            return View(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTransfer()
        {
            var clientId = GetCurrentClientId();
            var activeAccounts = (await _bankAccountService.GetAllAccountsForClient(clientId))
                .Where(x => x.Enabled && x.Approved.HasValue && x.Approved.Value).ToList();

            var vm = new BankTransactionViewModel
            {
                ActiveAccounts = new  List<SelectListItem>(),
                SourceClientId = 1
            };

            PopulateSelectListWithAccounts(activeAccounts, vm.ActiveAccounts);

            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransfer(BankTransactionViewModel transfer)
        {
            // Eur, 7856a896-25be-494c-829b-d97fc2f8c8ad, admin@a.a
            if (ModelState.IsValid)
            {
                transfer.SourceClientId = GetCurrentClientId();
                var result = await _bankTransferService.Send(transfer);
                if (result)
                {
                    return RedirectToAction(nameof(TransferSent));
                }
                else
                {
                    ModelState.AddModelError("", "Oh no, something went boo-boo!");
                }
            }

            var activeAccounts = (await _bankAccountService.GetAllAccountsForClient(transfer.SourceClientId))
                .Where(x => x.Enabled && x.Approved.HasValue && x.Approved.Value).ToList();
            PopulateSelectListWithAccounts(activeAccounts, transfer.ActiveAccounts);

            return View(transfer);
        }

        [HttpGet]
        public IActionResult TransferSent()
        {
            return View();
        }

        private void PopulateSelectListWithAccounts(List<BankAccount> accounts, List<SelectListItem> selectList)
        {
            if(selectList == null)
            {
                selectList = new List<SelectListItem>();
            }

            foreach (var account in accounts)
            {
                selectList.Add(new SelectListItem
                {
                    Value = account.AccountNumber,
                    Text = $"{account.AccountNumber} | {Enum.GetName(typeof(CurrencyTypes), account.Currency)} | Balance: {account.Balance}"
                });
            }
        }
    }
}