using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class LeaderBoardRewardStatus
	{
		public enum RewardStatus
		{
			NOT_THE_LEADER,
			LEADER_REWARD_OWNED,
			LEADER_REWARD_GRANTED,
			NOT_THE_LEADER_REWARD_OWNED
		}

		[JsonProperty("secondsTillNextCheck")]
		public int SecondsTillNextCheck { get; set; }

		[JsonProperty("status")]
		public RewardStatus Status { get; set; }
	}
}
