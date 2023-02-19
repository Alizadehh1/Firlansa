using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Firlansa.WebUI.AppCode.Providers
{
    public class CultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string lang = "az";

            string path = httpContext.Request.Path;

            Match match = Regex.Match(path, @"\/(?<lang>az|en|ru)\/?.*");

            if (match.Success)
            {
                lang = match.Groups["lang"].Value;

                httpContext.Response.Cookies.Delete("lang");
                httpContext.Response.Cookies.Append("lang", lang, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30)
                });

                return Task.FromResult(new ProviderCultureResult(lang, lang));
            }

            if (httpContext.Request.Cookies.TryGetValue("lang", out lang))
            {
                return Task.FromResult(new ProviderCultureResult(lang, lang));
            }

            return Task.FromResult(new ProviderCultureResult(lang, lang));
        }
    }
}
