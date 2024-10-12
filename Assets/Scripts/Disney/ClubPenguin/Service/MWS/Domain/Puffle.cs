using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class Puffle
	{
		[JsonProperty("_id")]
		public long PetId { get; set; }

		public int Type { get; set; }

		public string Name { get; set; }

		public long AdoptionDate { get; set; }

		public int Food { get; set; }

		public int Play { get; set; }

		public int Rest { get; set; }

		public int Clean { get; set; }

		public long HeadItemId { get; set; }

		public long Location { get; set; }
	}
}
