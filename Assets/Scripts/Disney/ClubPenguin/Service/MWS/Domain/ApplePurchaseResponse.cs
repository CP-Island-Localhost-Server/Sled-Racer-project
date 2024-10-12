using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class ApplePurchaseResponse
	{
		public class ProductDetails
		{
			[JsonProperty("product_type")]
			public string ProductType { get; set; }

			[JsonProperty("product_id")]
			public string ProductId { get; set; }
		}

		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("product_info")]
		public ProductDetails ProductInfo { get; set; }
	}
}
