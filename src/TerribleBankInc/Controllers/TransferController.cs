using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Controllers
{
    public class TransferController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBankTransferService _bankTransferService;

        public TransferController(IMapper mapper, IBankTransferService bankTransferService)
        {
            _mapper = mapper;
            _bankTransferService = bankTransferService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var transactions = await _bankTransferService.GetAllForAccount(accountId);

            return View(transactions);
        }
    }
}