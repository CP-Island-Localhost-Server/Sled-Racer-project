using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class GameEvent
	{
		public float time { get; set; }

		public int score { get; set; }

		[JsonProperty("event")]
		public Event eventType { get; set; }

		public string value { get; set; }
	}
}
