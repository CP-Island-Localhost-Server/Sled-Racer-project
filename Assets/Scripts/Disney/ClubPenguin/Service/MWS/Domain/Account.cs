namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class Account
	{
		public string Email { get; set; }

		public int Colour { get; set; }

		public int Language { get; set; }

		public string AccountType { get; set; }

		public bool Member { get; set; }

		public bool LapsedMember { get; set; }

		public string Username { get; set; }

		public long PlayerId { get; set; }

		public string PlayerSwid { get; set; }

		public int PenguinAge { get; set; }

		public bool PendingActivation { get; set; }

		public bool SafeMode { get; set; }

		public bool Recurring { get; set; }

		public int? DaysLeft { get; set; }

		public int? DaysAsMember { get; set; }

		public int? BadgeLevel { get; set; }

		public bool? Overriden { get; set; }

		public string OverrideType { get; set; }
	}
}
