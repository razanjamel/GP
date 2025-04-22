using Microsoft.AspNetCore.Mvc;

namespace GP.Admin
{
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
