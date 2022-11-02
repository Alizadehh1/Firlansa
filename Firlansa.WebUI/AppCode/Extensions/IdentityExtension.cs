using System.Linq;
using System.Security.Claims;

namespace Firlansa.WebUI.AppCode.Extensions
{
    static public partial class Extension
    {
        static public string GetPrincipalName(this ClaimsPrincipal principal)
        {
            string name = principal.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
            string surname = principal.Claims.FirstOrDefault(c => c.Type.Equals("surname"))?.Value;
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
            {
                return $"{name} {surname}";
            }

            return principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        }
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
        static public bool HasAccess(this ClaimsPrincipal principal,string policyName)
        {
            return principal.IsInRole("SuperAdmin")
                || principal.HasClaim(c => c.Type.Equals(policyName) && c.Value.Equals("1"));
        }
    }
}
