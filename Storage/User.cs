using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WeTube.Storage
{
	public class User
	{
		private readonly string id;
		private readonly string name;
		private readonly string avatar;

		public static readonly string[] Images =
		{
			"1f43b", "1f417", "1f431",
			"1f414", "1f42e", "1f98c",
			"1f436", "1f432", "1f985",
			"1f98a", "1f438", "1f992",
			"1f98d", "1f439", "1f434",
			"1f428", "1f981", "1f435",
			"1f42d", "1f43c", "1f437",
			"1f4a9", "1f430", "1f99d",
			"1f98f", "1f42f", "1f984",
			"1f43a", "1f993"
		};

		public static string RandomId
		{
			get
			{
				// TODO: Check if session is already in use
				var rng = new Random();
				var buffer = new byte[3]; // Digits / 2
				rng.NextBytes(buffer);
				return string.Concat(buffer.Select(x => x.ToString("x2")).ToArray());
			}
		}

		private static string RandomImage => Images[new Random().Next(Images.Length - 1)];

		public IEnumerable<Claim> Claims => new[]
		{
			new Claim("Name", name),
			new Claim("Avatar", avatar),
			new Claim("Id", id) 
		};

		// Creates or gets a user
		public User()
		{
			avatar = RandomImage;
			name   = $"Anonymous {ImageToName(avatar)}";
			id     = RandomId;
		}

		public static string ImageToName(string image)
		{
			switch (image)
			{
				case "1f43b": return "bear";
				case "1f417": return "boar";
				case "1f431": return "cat";
				case "1f414": return "chicken";
				case "1f42e": return "cow";
				case "1f98c": return "deer";
				case "1f436": return "dog";
				case "1f432": return "dragon";
				case "1f985": return "eagle";
				case "1f98a": return "fox";
				case "1f438": return "frog";
				case "1f992": return "giraffe";
				case "1f98d": return "gorilla";
				case "1f439": return "hamster";
				case "1f434": return "horse";
				case "1f428": return "koala";
				case "1f981": return "lion";
				case "1f435": return "monkey";
				case "1f42d": return "mouse";
				case "1f43c": return "panda";
				case "1f437": return "pig";
				case "1f4a9": return "poop";
				case "1f430": return "rabbit";
				case "1f99d": return "raccoon";
				case "1f98f": return "rhinoceros";
				case "1f42f": return "tiger";
				case "1f984": return "unicorn";
				case "1f43a": return "wolf";
				case "1f993": return "zebra";
				default: return "unknown";
			}
		}

		public override string ToString() => JsonConvert.SerializeObject(this);
	}
}