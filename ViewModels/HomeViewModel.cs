using GP.Models;
using System.Collections.Generic;

namespace GP.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public string SearchTerm { get; set; }

    }
}
