using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Firlansa.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static IActionContextAccessor AddModelError(this IActionContextAccessor ctx,string key,string message)
        {
            ctx.ActionContext.ModelState.AddModelError(key, message);
            return ctx;
        }

        public static bool ModelIsValid(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.IsValid;
        }

        public static string GetError(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.GetError();
        }

        public static string GetError(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return null;
            }
            return modelState.First().Value.Errors.First().ErrorMessage;
        }

    }
}
