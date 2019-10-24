using Microsoft.AspNetCore.Mvc;

namespace TerribleBankInc.Controllers
{
    public class BaseController : Controller
    {
        protected int GetCurrentUserId()
        {
            return int.Parse(User.Identity.Name);
        }
    }
}