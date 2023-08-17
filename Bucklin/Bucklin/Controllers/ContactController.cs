using Microsoft.AspNetCore.Mvc;

namespace Bucklin.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
