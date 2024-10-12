using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Disney.ClubPenguin.CPModuleUtils
{
	[RequireComponent(typeof(AudioSource), typeof(InputField))]
	public class KeyboardStateSounds : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		public AudioClip KeyboardOpenSound;

		public AudioClip KeyboardCloseSound;

		private bool wasOpenSoundPlayed;

		private bool hasFocus;

		public void OnSelect(BaseEventData eventData)
		{
			hasFocus = true;
		}

		public void OnDeselect(BaseEventData eventData)
		{
			hasFocus = false;
		}

		private void Update()
		{
			if (hasFocus && !wasOpenSoundPlayed && TouchScreenKeyboard.visible && KeyboardOpenSound != null)
			{
				PlayOpenSound();
				wasOpenSoundPlayed = true;
			}
			else if (wasOpenSoundPlayed && !TouchScreenKeyboard.visible && KeyboardCloseSound != null)
			{
				PlayCloseSound();
				wasOpenSoundPlayed = false;
			}
		}

		private void PlayOpenSound()
		{
			GetComponent<AudioSource>().PlayOneShot(KeyboardOpenSound);
		}

		private void PlayCloseSound()
		{
			GetComponent<AudioSource>().PlayOneShot(KeyboardCloseSound);
		}
	}
}
