using Microsoft.AspNetCore.Mvc;

namespace WeTube.Controllers
{
	public class CreateController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}