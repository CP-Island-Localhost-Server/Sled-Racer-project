using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class PuffleItem
	{
		[JsonProperty("_id")]
		public long Id { get; set; }

		[JsonProperty("qty")]
		public int Quantity { get; set; }
	}
}
