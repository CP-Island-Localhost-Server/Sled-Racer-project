using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(Toggle))]
	public class ToggleAudioTrigger : MonoBehaviour
	{
		public SFXEvent SFXToggleOn;

		public SFXEvent SFXToggleOff;

		public void TriggerAudio(bool _value)
		{
			if (base.enabled)
			{
				SFXEvent sFXEvent = (!_value) ? SFXToggleOff : SFXToggleOn;
				if (sFXEvent != 0)
				{
					Service.Get<IAudio>().SFX.Play(sFXEvent);
				}
			}
		}

		private void Start()
		{
			GetComponent<Toggle>().onValueChanged.AddListener(TriggerAudio);
		}

		private void OnDestroy()
		{
			GetComponent<Toggle>().onValueChanged.RemoveListener(TriggerAudio);
		}
	}
}
