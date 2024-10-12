using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class ApplePurchaseData
	{
		[JsonProperty("receiptId")]
		public string ReceiptId { get; set; }

		[JsonProperty("language")]
		public string Language { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("price")]
		public string Price { get; set; }

		[JsonProperty("bypass_security")]
		public int BypassSecurity { get; set; }
	}
}
