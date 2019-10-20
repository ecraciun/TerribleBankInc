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
        public async Task<IActionResult> ApproveAccount([FromBody] int accountId)
        {
            var result = await _bankAccountService.ApproveAccount(accountId);

            return RedirectToAction(nameof(GetAccountRequests));
        }

        [HttpPost]
        public async Task<IActionResult> RejectAccount([FromBody] int accountId, [FromBody] string reason)
        {
            var result = await _bankAccountService.RejectAccount(accountId, reason);

            return RedirectToAction(nameof(GetAccountRequests));
        }

        [HttpPost]
        public async Task<IActionResult> EnableAccount([FromBody] int accountId)
        {
            var result = await _bankAccountService.EnableAccount(accountId);

            return RedirectToAction(nameof(GetBlockedAccounts));
        }
    }
}