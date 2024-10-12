namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class AuthData
	{
		public long PlayerId { get; set; }

		public string PlayerSwid { get; set; }

		public string Username { get; set; }

		public string DisplayName { get; set; }

		public string AuthToken { get; set; }

		public string LastLogin { get; set; }

		public bool Member { get; set; }

		public bool PendingActivation { get; set; }

		public bool SaveMode { get; set; }

		public string AccountType { get; set; }

		public int? DaysLeft { get; set; }
	}
}
