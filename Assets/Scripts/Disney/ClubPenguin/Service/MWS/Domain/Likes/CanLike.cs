using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class CanLike
	{
		[JsonProperty("canLike")]
		public bool Value { get; set; }

		public long NextLike_msecs { get; set; }

		public Periodicity Periodicity { get; set; }
	}
}
