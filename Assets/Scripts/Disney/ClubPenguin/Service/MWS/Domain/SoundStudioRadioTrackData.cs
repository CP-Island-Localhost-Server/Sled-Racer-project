using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class SoundStudioRadioTrackData
	{
		[JsonProperty("swid")]
		public string playerSwid { get; set; }

		[JsonProperty("playerDisplayName")]
		public string playerDisplayName { get; set; }

		[JsonProperty("soundStudioTrackData")]
		public SoundStudioTrackData soundStudioTrackData { get; set; }

		public override string ToString()
		{
			return string.Format("playerDisplayName: {0}, playerSwid: {1}, soundStudioTrackData: {2}", playerDisplayName, playerSwid, soundStudioTrackData);
		}
	}
}
