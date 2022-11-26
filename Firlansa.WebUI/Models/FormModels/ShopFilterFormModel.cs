using System.Collections.Generic;

namespace Firlansa.WebUI.Models.FormModels
{
    public class ShopFilterFormModel
    {
        public List<int> Sizes { get; set; }
        public List<int> Colors { get; set; }
        public List<int> Categories { get; set; }
    }
}
