using System.Collections.Generic;
using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class GetProductsResponse
	{
		[JsonProperty("isAgeGateRegion")]
		public bool IsAgeGateRegion { get; set; }

		[JsonProperty("productIds")]
		public IList<ProductDetail> Products { get; set; }

		[JsonProperty("ageGateType")]
		public string AgeGateType { get; set; }
	}
}
