using ElmahCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TerribleBankInc.Filters;

public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        context.HttpContext.RiseError(context.Exception);
    }
}
