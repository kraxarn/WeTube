using Microsoft.AspNetCore.Mvc;

namespace WeTube.Controllers
{
	public class BrowseController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}