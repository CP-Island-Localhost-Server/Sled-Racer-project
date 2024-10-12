using System.Collections.Generic;
using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class LeaderBoardResponse
	{
		[JsonProperty("players")]
		public List<LeaderBoardHighScore> Players { get; set; }

		[JsonProperty("countdown")]
		public int Countdown { get; set; }
	}
}
