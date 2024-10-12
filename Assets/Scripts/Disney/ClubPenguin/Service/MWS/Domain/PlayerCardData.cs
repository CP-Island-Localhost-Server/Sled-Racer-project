using System.Collections.Generic;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class PlayerCardData
	{
		public string Name { get; set; }

		public long Coins { get; set; }

		public int DaysAsMember { get; set; }

		public int BadgeLevel { get; set; }

		public int TotalLikesReceived { get; set; }

		public int TotalLikesGiven { get; set; }

		public int RemainingAwardGames { get; set; }

		public int PenguinAge { get; set; }

		public IDictionary<string, IList<int>> Items { get; set; }

		public IList<IDictionary<string, string>> Outfits { get; set; }

		public bool Member { get; set; }
	}
}
