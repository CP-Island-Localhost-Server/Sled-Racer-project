using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class Count
	{
		[JsonProperty("count")]
		public int Value { get; set; }

		public int MaxCount { get; set; }

		public int AccumCount { get; set; }
	}
}
