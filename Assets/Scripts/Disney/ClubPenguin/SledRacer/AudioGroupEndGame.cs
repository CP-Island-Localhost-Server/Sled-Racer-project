namespace Disney.ClubPenguin.SledRacer
{
	public class AudioGroupEndGame : AudioGroup
	{
		public AudioGroupEndGame()
			: base("Audio_All_SFX_UIAudio_EndGameScreen")
		{
		}

		public void StopAll()
		{
			Group.Stop();
		}
	}
}
