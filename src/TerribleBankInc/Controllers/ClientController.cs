using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TerribleBankInc.Data;
using TerribleBankInc.Models;
using TerribleBankInc.Models.ViewModels;
using TerribleBankInc.Services;
using TerribleBankInc.Services.Interfaces;

namespace TerribleBankInc.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Client/Edit/5
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
    }
}