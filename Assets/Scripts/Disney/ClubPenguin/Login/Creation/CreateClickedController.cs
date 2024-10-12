using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.Login.Creation
{
	public class CreateClickedController : MonoBehaviour
	{
		public delegate void CreateButtonClickedDelegate();

		public CreateButtonClickedDelegate CreateButtonClicked;

		public Button CreateButton;

		public AudioClip ClickSound;

		[HideInInspector]
		public AudioSource RootAudioSource;

		private void Awake()
		{
			CreateButton.onClick.AddListener(delegate
			{
				RootAudioSource.PlayOneShot(ClickSound);
				if (CreateButtonClicked != null)
				{
					CreateButtonClicked();
				}
			});
		}
	}
}
