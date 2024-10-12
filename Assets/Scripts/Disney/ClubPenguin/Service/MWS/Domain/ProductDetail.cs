using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class ProductDetail
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("duration")]
		public string Duration { get; set; }

		[JsonProperty("productType")]
		public string ProductType { get; set; }
	}
}
