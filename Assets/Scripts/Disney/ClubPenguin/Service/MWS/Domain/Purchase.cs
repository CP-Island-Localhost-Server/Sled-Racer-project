using System;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class Purchase
	{
		public long id { get; set; }

		public long playerId { get; set; }

		public long catalogId { get; set; }

		public string itemType { get; set; }

		public long itemId { get; set; }

		public int cost { get; set; }

		public DateTime purchaseDate { get; set; }
	}
}
