using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace WeTube.Storage
{
	public abstract class Cookie
	{
		public static async void RefreshCookie(HttpContext context)
		{
			var items = context.User.Claims.Count();
			var claims = items <= 0 ? new User().Claims : context.User.Claims;

			await context.SignOutAsync();
			await context.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(new ClaimsIdentity(claims)),
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTime.UtcNow.AddYears(1)
				}
			);
		}

		public static async void SignOut(HttpContext context) => await context.SignOutAsync();

		public static async void RefreshCookie(HttpContext context, IIdentity identity)
		{
			await context.SignOutAsync();
			await context.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(new ClaimsIdentity(identity)),
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTime.UtcNow.AddYears(1)
				}
			);
		}

		public static UserValues GetCurrentUser(HttpContext context)
		{
			return new UserValues
			{
				Id     = GetUserValue(context, "Id"),
				Name   = GetUserValue(context, "Name"),
				Avatar = GetUserValue(context, "Avatar")
			};
		}

		public static string GetUserValue(HttpContext context, string type) =>
			context.User.Claims.FirstOrDefault(x => x.Type == type)?.Value;

		public static bool SetUserValue(HttpContext context, IEnumerable<Claim> claims, string type, string value)
		{
			var claimsIdentity = new ClaimsIdentity(claims);
			if (!claimsIdentity.TryRemoveClaim(claimsIdentity.FindFirst(type)))
				return false;
			claimsIdentity.AddClaim(new Claim(type, value));
			RefreshCookie(context, claimsIdentity);
			return true;
		}
	}
}