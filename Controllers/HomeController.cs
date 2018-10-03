using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WeTube.Storage;

namespace WeTube.Controllers
{
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
			RefreshCookie();

	        var user = new UserValues
	        {
				Id     = GetUserValue("Id"),
				Name   = GetUserValue("Name"),
				Avatar = GetUserValue("Avatar")
			};

			return View(user);
        }

	    public IActionResult Error(int code)
	    {
		    ViewData["Code"] = code;
			return View();
	    }

	    private async void RefreshCookie()
	    {
		    var items = HttpContext.User.Claims.Count();
			var claims = items <= 0 ? new User().Claims : HttpContext.User.Claims;

			await HttpContext.SignOutAsync();
		    await HttpContext.SignInAsync(
			    CookieAuthenticationDefaults.AuthenticationScheme,
			    new ClaimsPrincipal(new ClaimsIdentity(claims)),
			    new AuthenticationProperties
			    {
				    IsPersistent = true,
				    ExpiresUtc = DateTime.UtcNow.AddYears(1)
			    }
		    );
		}
		
	    public string GetUserValue(string type) =>
		    HttpContext.User.Claims.FirstOrDefault(x => x.Type == type)?.Value;
	}
}