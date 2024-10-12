namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class ID
	{
		public string Id { get; set; }

		public long Time { get; set; }

		public int Count { get; set; }

		public bool IsFriend { get; set; }

		public bool IsBestFriend { get; set; }

		public ID()
		{
		}

		public ID(string id)
		{
			Id = id;
		}
	}
}
