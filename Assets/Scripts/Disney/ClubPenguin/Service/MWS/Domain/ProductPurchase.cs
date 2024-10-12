using System.Collections.Generic;
using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class ProductPurchase
	{
		[JsonProperty("productId")]
		public string ProductId { get; set; }

		[JsonProperty("store")]
		public StoreType? Store { get; set; }

		[JsonProperty("purchaseDate")]
		public long PurchaseDate { get; set; }

		[JsonProperty("purchasedItems")]
		public IDictionary<string, object> PurchasedItems { get; set; }
	}
}
