using Microsoft.AspNetCore.Mvc;

namespace GP.Controllers
{
    public class PayController : Controller
    {
        public IActionResult ShopCart()
        {
            return View();
        }
    }
}
