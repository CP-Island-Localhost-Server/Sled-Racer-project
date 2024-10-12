using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class LeaderBoardHighScore
	{
		[JsonProperty("rank")]
		public int Rank { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("score")]
		public int Score { get; set; }

		[JsonProperty("playerId")]
		public int PlayerId { get; set; }

		[JsonProperty("playerSWID")]
		public string PlayerSWID { get; set; }

		[JsonProperty("colour")]
		public int Colour { get; set; }

		[JsonProperty("friend")]
		public bool? IsFriend { get; set; }

		[JsonProperty("hasRewardItem")]
		public bool? HasRewardItem { get; set; }
	}
}
