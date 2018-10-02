using System.Collections.Generic;

namespace WeTube.Storage
{
	public abstract class RoomUser
	{
		public enum RoomUserType
		{
			Admin,
			Moderator
		}

		public string Id;
		public RoomUserType Type;

		public override string ToString() => Id;
	}

	public class Room
	{
		public string Name;
		public string Id;
		public string Owner;
		public List<RoomUser> Users;

		public Room() => Users = new List<RoomUser>();

		public override string ToString() => Id;
	}
}