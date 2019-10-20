using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IMapper mapper, IBankAccountService bankAccountService)
        {
            _mapper = mapper;
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int clientId)
        {
            //TODO: remove hardcoding
            var accounts = await _bankAccountService.GetAllAccountsForClient(clientId);
            var vm = new ClientBankAccountsViewModel
            {
                ActiveAccounts = accounts.Where(x => x.Approved.HasValue && x.Approved.Value && x.Enabled).OrderBy(x => x.Currency).ToList(),
                DisabledAccounts = accounts.Where(x => !x.Enabled && x.Approved.HasValue && x.Approved.Value).OrderBy(x => x.Currency).ToList(),
                PendingAccounts =  accounts.Where(x => !x.Approved.HasValue && !x.Enabled).OrderBy(x => x.Currency).ToList(),
                RejectedAccounts = accounts.Where(x => x.Approved.HasValue && x.Approved.Value == false).OrderBy(x => x.Currency).ToList()
            };
            return View(vm);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var account = await _bankAccountService.GetById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> RequestNew()
        {
            //TODO: remove hardcoding
            return View(new NewBankAccountRequestViewModel{ ClientId = 1});
        }

        [HttpPost]
        public async Task<IActionResult> RequestNew(NewBankAccountRequestViewModel newBankAccountRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _bankAccountService.RequestNewBankAccount(newBankAccountRequest);

                return RedirectToAction(nameof(Index));
            }
            return View(newBankAccountRequest);
        }

        [HttpGet]
        public async Task<IActionResult> BlockAccount(int id)
        {
            var account = await _bankAccountService.GetById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> BlockAccountConfirm(int ID)
        {
            var result = await _bankAccountService.BlockAccount(ID);
            return RedirectToAction(nameof(Index));
        }
    }
}