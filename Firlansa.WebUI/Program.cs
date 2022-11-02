using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firlansa.WebUI
{
    public class Program
    {
        static internal string[] principals = null;
        public static void Main(string[] args)
        {
            var types = typeof(Program).Assembly.GetTypes();

            principals = types
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t.IsDefined(typeof(AuthorizeAttribute), true))
                .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>())
                .ToArray()
                .Union(
                types
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic
                && !method.IsDefined(typeof(NonActionAttribute), true)
                && method.IsDefined(typeof(AuthorizeAttribute), true))
                .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>())
                )
                .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
                .SelectMany(a => a.Policy.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries))
                .Distinct()
                .ToArray();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
