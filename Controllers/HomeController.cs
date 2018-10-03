using Microsoft.AspNetCore.Mvc;
using WeTube.Storage;

namespace WeTube.Controllers
{
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
	        Cookie.RefreshCookie(HttpContext);
			return View(Cookie.GetCurrentUser(HttpContext));
        }

	    public IActionResult Error(int code)
	    {
		    ViewData["Code"] = code;
			return View();
	    }
    }
}