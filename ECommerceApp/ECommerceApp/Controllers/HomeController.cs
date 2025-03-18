using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity.Name;
                ViewData["UserName"] = userName;
            }
            return View();
        }
    }
}
