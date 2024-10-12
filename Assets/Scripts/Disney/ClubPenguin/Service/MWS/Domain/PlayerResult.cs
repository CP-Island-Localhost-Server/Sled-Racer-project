using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class PlayerResult
	{
		[JsonProperty("playerId")]
		public long PlayerId { get; set; }

		[JsonProperty("team")]
		public string Team { get; set; }

		[JsonProperty("result")]
		public string Result { get; set; }
	}
}
