using System.Runtime.Serialization;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public enum Event
	{
		[EnumMember(Value = "left start")]
		LEFT_START,
		[EnumMember(Value = "left end")]
		LEFT_END,
		[EnumMember(Value = "right start")]
		RIGHT_START,
		[EnumMember(Value = "right end")]
		RIGHT_END,
		[EnumMember(Value = "jump")]
		JUMP,
		[EnumMember(Value = "random")]
		RANDOM,
		[EnumMember(Value = "pause start")]
		PAUSE_START,
		[EnumMember(Value = "pause end")]
		PAUSE_END,
		[EnumMember(Value = "end")]
		END
	}
}
