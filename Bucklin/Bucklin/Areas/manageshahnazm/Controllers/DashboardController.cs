using Microsoft.AspNetCore.Mvc;

namespace Bucklin.Areas.manageshahnazm.Controllers
{
    [Area("ManageShahnazm")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
