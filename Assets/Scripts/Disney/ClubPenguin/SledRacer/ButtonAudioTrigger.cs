using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(Button))]
	public class ButtonAudioTrigger : MonoBehaviour
	{
		public SFXEvent defaultAudio;

		public void TriggerAudio()
		{
			SFXEvent sFXEvent = defaultAudio;
			if (sFXEvent != 0)
			{
				Service.Get<IAudio>().SFX.Play(sFXEvent);
			}
		}

		private void Start()
		{
			GetComponent<Button>().onClick.AddListener(TriggerAudio);
		}

		private void OnDestroy()
		{
			GetComponent<Button>().onClick.RemoveListener(TriggerAudio);
		}
	}
}
