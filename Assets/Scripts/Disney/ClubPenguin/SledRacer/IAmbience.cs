namespace Disney.ClubPenguin.SledRacer
{
	public interface IAmbience : IAudioGroup
	{
		void Stop();

		void Play(AmbienceTrack track);
	}
}
