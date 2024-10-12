using System.Collections.Generic;
using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	internal class ListPlayerSwids
	{
		[JsonProperty("swids")]
		public List<string> Swids { get; set; }

		public override string ToString()
		{
			return string.Format("Swids: {0}", Swids);
		}
	}
}
