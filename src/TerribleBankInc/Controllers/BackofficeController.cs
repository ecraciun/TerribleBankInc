using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Controllers
{
    public class BackofficeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountService _bankAccountService;
        private readonly IClientService _clientService;

        public BackofficeController(IMapper mapper, IBankAccountService bankAccountService, IClientService clientService)
        {
            _mapper = mapper;
            _bankAccountService = bankAccountService;
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountRequests()
        {
            var accounts = await _bankAccountService.GetPendingAccounts();
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlockedAccounts()
        {
            var accounts = await _bankAccountService.GetDisabledAccounts();
            return View(accounts);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewAccount(int accountId)
        {
            var account = await _bankAccountService.GetById(accountId);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAccount(int ID)
        {
            var result = await _bankAccountService.ApproveAccount(ID);

            return RedirectToAction(nameof(GetAccountRequests));
        }

        [HttpPost]
        public async Task<IActionResult> RejectAccount(int ID, [FromBody] string Reason)
        {
            var result = await _bankAccountService.RejectAccount(ID, Reason);

            return RedirectToAction(nameof(GetAccountRequests));
        }

        [HttpPost]
        public async Task<IActionResult> EnableAccount(int ID)
        {
            var result = await _bankAccountService.EnableAccount(ID);

            return RedirectToAction(nameof(GetBlockedAccounts));
        }
    }
}