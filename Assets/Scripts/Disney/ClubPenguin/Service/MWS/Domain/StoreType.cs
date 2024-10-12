using System.Runtime.Serialization;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public enum StoreType
	{
		[EnumMember(Value = "apple")]
		APPLE,
		[EnumMember(Value = "google_play")]
		GOOGLE_PLAY
	}
}
