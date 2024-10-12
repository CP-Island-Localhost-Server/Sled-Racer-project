namespace Disney.ClubPenguin.SledRacer
{
	public interface IMusic : IAudioGroup
	{
		MusicTrack CurrentTrack
		{
			get;
		}

		void Stop();

		void Play(MusicTrack track);
	}
}
