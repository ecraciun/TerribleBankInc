using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet]
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
            foreach (var account in activeAccounts)
            {
                vm.ActiveAccounts.Add(new SelectListItem
                {
                    Value = account.AccountNumber,
                    Text = $"{account.AccountNumber} | {Enum.GetName(typeof(CurrencyTypes), account.Currency)} | Balance: {account.Balance}"
                });
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransfer(BankTransactionViewModel transfer)
        {
            if (ModelState.IsValid)
            {
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

            return View(transfer);
        }

        [HttpGet]
        public async Task<IActionResult> TransferSent()
        {
            return View();
        }
    }
}