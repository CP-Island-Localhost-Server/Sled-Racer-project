namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class IglooVillageEntry
	{
		public long OwnerId { get; set; }

		public string OwnerSwid { get; set; }

		public string OwnerName { get; set; }

		public string DisplayName { get; set; }

		public string IglooName { get; set; }

		public int FriendsCount { get; set; }

		public int LikesCount { get; set; }

		public string WorldName { get; set; }

		public long ActiveLayoutId { get; set; }

		public long LocationId { get; set; }

		public long BuildingId { get; set; }

		public bool IsOwnerMember { get; set; }

		public bool IsOwnerBestFriend { get; set; }

		public int Population { get; set; }

		public int PopularityScore { get; set; }
	}
}
