using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.RegularExpressions;

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
        public static string GetCurrentCulture(this HttpContext ctx)
        {
            var match = Regex.Match(ctx.Request.Path, @"\/(?<lang>az|en|ru)\/?.*");

            if (match.Success)
                return match.Groups["lang"].Value;

            if (ctx.Request.Cookies.TryGetValue("lang", out string lang))
                return lang;

            return "az";
        }

    }
}
