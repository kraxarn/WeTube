using System.IO;
using Newtonsoft.Json;

namespace WeTube.Storage
{
	public abstract class Config
	{
		public static string ApiGoogle;

		/// <summary>
		/// Loads config file from data/config.json
		/// </summary>
		public static void Load()
		{
			if (File.Exists("data/config.json"))
			{
				dynamic json = JsonConvert.DeserializeObject(File.ReadAllText("data/config.json"));
				ApiGoogle = json.Api.Google;
			}
		}
	}
}