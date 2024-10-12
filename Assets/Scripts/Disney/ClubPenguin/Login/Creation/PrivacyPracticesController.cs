using Disney.ClubPenguin.CPModuleUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.Login.Creation
{
	public class PrivacyPracticesController : MonoBehaviour
	{
		public delegate void ClosedDelegate();

		public ClosedDelegate Closed;

		public Button CloseButton;

		public AudioClip OkButtonAudioClip;

		public AudioClip CloseButtonAudioClip;

		[HideInInspector]
		public AudioSource RootAudioSource;

		public void Awake()
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton, visible: false);
		}

		public void OnOkPressed()
		{
			if (RootAudioSource != null)
			{
				RootAudioSource.PlayOneShot(OkButtonAudioClip);
			}
			close();
		}

		public void OnCloseButtonPressed()
		{
			if (RootAudioSource != null)
			{
				RootAudioSource.PlayOneShot(CloseButtonAudioClip);
			}
			close();
		}

		private void close()
		{
			if (Closed != null)
			{
				Closed();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
