using Microsoft.AspNetCore.Mvc;
using Bucklin.Models;

namespace Bucklin.Areas.manageshahnazm.Controllers
{
    [Area("manageshahnazm")]

    public class SettingsController : Controller
    {

        public IActionResult Index()
        {
            
            return View();
        }
    }
}
