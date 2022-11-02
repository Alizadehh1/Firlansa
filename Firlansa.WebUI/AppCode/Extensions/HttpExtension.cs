using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Firlansa.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static string GetAppLink(this IActionContextAccessor ctx)
        {
            string host = ctx.ActionContext.HttpContext.Request.Host.ToString();
            string scheme = ctx.ActionContext.HttpContext.Request.Scheme;
            return $"{scheme}://{host}";
        }

    }
}
