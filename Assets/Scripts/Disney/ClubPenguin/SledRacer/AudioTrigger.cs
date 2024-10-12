using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class AudioTrigger : MonoBehaviour
	{
		public SFXEvent defaultAudio;

		public void TriggerAudio(SFXEvent clip = SFXEvent.None)
		{
			SFXEvent sFXEvent = (clip == SFXEvent.None) ? defaultAudio : clip;
			if (sFXEvent != 0)
			{
				Service.Get<IAudio>().SFX.Play(sFXEvent);
			}
		}
	}
}
