using Disney.ClubPenguin.Service.MWS.Domain;

namespace Disney.ClubPenguin.SledRacer
{
	public class PlayerData
	{
		public Account Account
		{
			get;
			set;
		}

		public HighScore HighScore
		{
			get;
			set;
		}

		public LeaderBoardRewardStatus.RewardStatus RewardStatus
		{
			get;
			set;
		}

		public int LastScore
		{
			get;
			set;
		}

		public long LastGameCoinsEarned
		{
			get;
			set;
		}

		public long TotalCoins
		{
			get;
			set;
		}

		public bool hasTrophy
		{
			get;
			set;
		}

		public PlayerData()
		{
			Account = new Account();
			HighScore = new HighScore();
			hasTrophy = false;
			RewardStatus = LeaderBoardRewardStatus.RewardStatus.NOT_THE_LEADER;
		}
	}
}
