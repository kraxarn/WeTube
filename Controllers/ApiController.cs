﻿using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using WeTube.Storage;

namespace WeTube.Controllers
{
	public class ApiController : Controller
	{
		private string GetUserValue(string type) => 
			HttpContext.User.Claims.FirstOrDefault(x => x.Type == type)?.Value;

		private bool SetUserValue(string type, string value)
		{
			var claims = new ClaimsIdentity(User.Claims);
			if (!claims.TryRemoveClaim(claims.FindFirst(type)))
				return false;
			claims.AddClaim(new Claim(type, value));
			ReSignIn(claims);
			return true;
		}

		private async void ReSignIn(IIdentity identity)
		{
			await HttpContext.SignOutAsync();
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(new ClaimsIdentity(identity)),
				new AuthenticationProperties
				{
					IsPersistent = true,
					ExpiresUtc = DateTime.UtcNow.AddYears(1)
				}
			);
		}

		private async void SignOutAsync() => await HttpContext.SignOutAsync();

		private static JsonResult GetResponse(bool err, string msg)
		{
			return new JsonResult(new
			{
				error = err,
				message = msg
			});
		}

		public IActionResult Index()
		{
			return new JsonResult(new
			{
				error = true,
				message = "Invalid request"
			});
		}

		public IActionResult SetUserInfo(string name, string avatar)
		{
			if (name != null)
			{
				if (!SetUserValue("Name", name))
					return GetResponse(true, "Failed to update name");
			}

			if (avatar != null)
			{
				if (!SetUserValue("Avatar", avatar))
					return GetResponse(true, "Failed to update avatar");
			}

			return GetResponse(false, null);
		}

		public IActionResult SignOut()
		{
			SignOutAsync();
			return GetResponse(false, null);
		}

		public IActionResult CreateRoom(string name)
		{
			// Check if name is empty
			if (name == null)
				return GetResponse(true, "No name specified");

			// Check if shorter than 3 chars
			if (name.Length < 3)
				return GetResponse(true, "Name too short");

			// Check if longer than 32 chars
			if (name.Length > 32)
				return GetResponse(true, "Name too long");

			// Format name
			// TODO: Format emojis etc.
			var id = name.ToLower().Replace(" ", "");

			// Check if name (id) is already taken
			if (System.IO.File.Exists($"data/rooms/{id}.json"))
				return GetResponse(true, "Name already taken");

			// Try to save it
			var room = new Room
			{
				Id    = id,
				Name  = name,
				Owner = GetUserValue("Id")
			};

			try
			{
				System.IO.File.WriteAllText($"data/rooms/{id}.json", JsonConvert.SerializeObject(room));
			}
			catch (IOException)
			{
				return GetResponse(true, "Invalid name");
			}

			return GetResponse(false, id);
		}
		/*
		 * Example room
		 *	{
		 *		"name": "<Room name>",
		 *		"id": "<Name without spaces etc.>",
		 *		"admins": [
		 *			{
		 *				"id": "<User ID>",
		 *				"type": "<Type, admin/mod etc.>"
		 *			}
		 *		]
		 *	}
		 */
	}
}