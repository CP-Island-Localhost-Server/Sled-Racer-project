using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Disney.ClubPenguin.CPModuleUtils
{
	[RequireComponent(typeof(IPointerClickHandler), typeof(AudioSource))]
	public class PlaySoundOnClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public delegate void OnAudioCompleteDelegate();

		public OnAudioCompleteDelegate OnAudioComplete;

		public AudioClip OnClickAudioClip;

		private bool _IsPlaying;

		public bool IsPlaying => _IsPlaying;

		public void OnPointerClick(PointerEventData eventData)
		{
			_IsPlaying = true;
			GetComponent<AudioSource>().PlayOneShot(OnClickAudioClip);
			StartCoroutine(WaitForAudioCompletion());
		}

		private IEnumerator WaitForAudioCompletion()
		{
			yield return new WaitForSeconds(OnClickAudioClip.length);
			_IsPlaying = false;
			if (OnAudioComplete != null)
			{
				OnAudioComplete();
			}
		}
	}
}
