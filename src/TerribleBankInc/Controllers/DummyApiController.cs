using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TerribleBankInc.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DummyApiController : ControllerBase
{
    // GET: api/DummyApi
    [HttpGet]
    [ProducesResponseType(200)]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET: api/DummyApi/5
    [ProducesResponseType(200)]
    [HttpGet("{id}", Name = "Get")]
    public string Get(int id)
    {
        return "value";
    }

    // POST: api/DummyApi
    [HttpPost]
    public void Post([FromBody] string value) { }

    // PUT: api/DummyApi/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value) { }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public void Delete(int id) { }
}
