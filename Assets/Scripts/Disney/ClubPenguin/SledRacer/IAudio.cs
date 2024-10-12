namespace Disney.ClubPenguin.SledRacer
{
	public interface IAudio : IAudioGroup
	{
		IMusic Music
		{
			get;
		}

		ISFX SFX
		{
			get;
		}

		IAmbience Ambience
		{
			get;
		}
	}
}
