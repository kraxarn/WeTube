using Microsoft.AspNetCore.Mvc;

namespace WeTube.Controllers
{
	public class RoomsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult View(string id)
		{
			return View();
		}
	}
}