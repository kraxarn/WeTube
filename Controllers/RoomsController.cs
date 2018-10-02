using Microsoft.AspNetCore.Mvc;

namespace WeTube.Controllers
{
	public class RoomsController : Controller
	{
		public IActionResult Index(string id)
		{
			return View();
		}
	}
}