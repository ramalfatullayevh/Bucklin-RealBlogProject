using Microsoft.AspNetCore.Mvc;

namespace Bucklin.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
