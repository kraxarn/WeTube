using Microsoft.AspNetCore.Mvc;
using WeTube.Storage;

namespace WeTube.Controllers
{
	public class RoomsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public new IActionResult View(string id)
		{
			ViewData["RoomId"] = id;
			return View(CookieManager.GetCurrentUser(HttpContext));
		}
	}
}