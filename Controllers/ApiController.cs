﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WeTube.Storage;

namespace WeTube.Controllers
{
	public class ApiController : Controller
	{
		#region Helpers

		private bool SetUserValue(string type, string value) => 
			CookieManager.SetUserValue(HttpContext, User.Claims, type, value);

		private static JsonResult GetResponse(bool err, string msg)
		{
			return new JsonResult(new
			{
				error = err,
				message = msg
			});
		}

		#endregion

		public IActionResult Index()
		{
			return new JsonResult(new
			{
				error = true,
				message = "Invalid request"
			});
		}

		#region Account

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
			CookieManager.SignOut(HttpContext);
			return GetResponse(false, null);
		}

		#endregion

		#region Rooms
		
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
				Owner = CookieManager.GetUserValue(HttpContext, "Id")
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

		#endregion

		#region YouTube

		public struct YoutubeVideo
		{
			public string Id, Title, Description, Thumbnail;
		}

		public IActionResult Search(string q)
		{
			if (q == null)
				return GetResponse(true, "Missing query");

			using (var client = new WebClient())
			{
				dynamic info = JsonConvert.DeserializeObject(client.DownloadString($"https://www.googleapis.com/youtube/v3/search?part=snippet&results=5&q={q}&type=video&key={Config.ApiGoogle}"));

				var videos = new List<YoutubeVideo>();

				foreach (var item in info.items)
				{
					videos.Add(new YoutubeVideo
					{
						Id = item.id.videoId,
						Title = item.snippet.title,
						Description = item.snippet.description,
						Thumbnail = item.snippet.thumbnails.medium.url
					});
				}

				return new JsonResult(new
				{
					error = false,
					message = videos
				});
			}
		}

		#endregion
	}
}