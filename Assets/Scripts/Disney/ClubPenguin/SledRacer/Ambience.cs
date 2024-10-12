using Fabric;

namespace Disney.ClubPenguin.SledRacer
{
	public class Ambience : AudioGroup, IAmbience, IAudioGroup
	{
		private string mainAmbienceEvent;

		public Ambience()
			: base("Audio_All_SFX_Ambience")
		{
			mainAmbienceEvent = AmbienceEvent.AMB_Main.ToString();
		}

		public void Stop()
		{
			events.PostEvent(mainAmbienceEvent, EventAction.StopSound);
		}

		public void Play(AmbienceTrack track)
		{
			events.PostEvent(mainAmbienceEvent, EventAction.StopSound);
			events.PostEvent(mainAmbienceEvent, EventAction.SetSwitch, track.ToString());
			events.PostEvent(mainAmbienceEvent);
		}
	}
}
