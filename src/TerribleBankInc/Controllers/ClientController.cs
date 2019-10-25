using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TerribleBankInc.Models.Entities;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        private readonly IClientService _clientService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper, IAuthenticationService authenticationService)
        {
            _clientService = clientService;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                id = GetCurrentClientId();
            }

            var client = await _clientService.Get(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ClientViewModel>(client));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientService.Get(id.Value);

            if (client == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<ClientViewModel>(client));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ClientController.Details), new { id });
            }
            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsername()
        {
            var clientId = GetCurrentClientId();
            var user = await _authenticationService.GetUserByClientId(clientId);
            return View(_mapper.Map<UserProfileViewModel>(user));
        }

        [HttpPost]
        public async Task<IActionResult> EditUsername(UserProfileViewModel vm)
        {
            return View(vm);
        }
    }
}