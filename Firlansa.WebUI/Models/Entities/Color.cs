using Firlansa.WebUI.AppCode.Infrastructure;

namespace Firlansa.WebUI.Models.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public string HexCode { get; set; }
    }
}
