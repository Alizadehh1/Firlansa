using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.AppCode.Infrastructure
{
    public class CommandJsonResponse
    {
        public CommandJsonResponse()
        {

        }
        public CommandJsonResponse(bool error,string message)
        {
            this.Error = error;
            this.Message = message;
        }
        public bool Error { get; set; }
        public string Message { get; set; }
    }
}
