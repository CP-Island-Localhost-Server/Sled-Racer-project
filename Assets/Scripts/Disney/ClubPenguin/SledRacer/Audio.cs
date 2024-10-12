namespace Disney.ClubPenguin.SledRacer
{
	public class Audio : AudioGroup, IAudio, IAudioGroup
	{
		public IMusic Music
		{
			get;
			private set;
		}

		public ISFX SFX
		{
			get;
			private set;
		}

		public IAmbience Ambience
		{
			get;
			private set;
		}

		public Audio()
			: base("Audio_All")
		{
			Music = new Music();
			SFX = new SFX();
			Ambience = new Ambience();
		}

		public new void Mute()
		{
			Group.Mute = true;
		}

		public new void UnMute()
		{
			Group.FadeIn(0.5f, 0.5f);
			Group.Mute = false;
		}
	}
}
