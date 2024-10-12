using Fabric;

namespace Disney.ClubPenguin.SledRacer
{
	public class Music : AudioGroup, IAudioGroup, IMusic
	{
		private string mainMusicEvent;

		private MusicTrack currentTrack;

		public MusicTrack CurrentTrack => currentTrack;

		public Music()
			: base("Audio_All_Music")
		{
			mainMusicEvent = MusicEvent.MUS_Main.ToString();
		}

		public void Stop()
		{
			events.PostEvent(mainMusicEvent, EventAction.StopSound);
			currentTrack = MusicTrack.None;
		}

		public void Play(MusicTrack track)
		{
			if (track != currentTrack)
			{
				currentTrack = track;
				events.PostEvent(mainMusicEvent, EventAction.StopSound);
				events.PostEvent(mainMusicEvent, EventAction.SetSwitch, track.ToString());
				events.PostEvent(mainMusicEvent);
			}
		}
	}
}
