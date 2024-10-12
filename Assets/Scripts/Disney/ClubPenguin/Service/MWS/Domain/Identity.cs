namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class Identity
	{
		public long PlayerId { get; set; }

		public string PlayerSwid { get; set; }

		public string DisplayName { get; set; }

		public string AccountType { get; set; }

		public int DaysLeft { get; set; }

		public int BadgeLevel { get; set; }

		public bool Member { get; set; }

		public string Username { get; set; }
	}
}
