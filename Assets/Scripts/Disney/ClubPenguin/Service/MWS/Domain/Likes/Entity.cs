namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class Entity
	{
		public string Id { get; set; }

		public LikedBy Likedby { get; set; }

		public Likes Likes { get; set; }
	}
}
