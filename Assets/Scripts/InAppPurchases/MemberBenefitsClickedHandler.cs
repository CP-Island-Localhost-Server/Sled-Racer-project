using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	[RequireComponent(typeof(AudioSource))]
	public class MemberBenefitsClickedHandler : MonoBehaviour
	{
		public delegate void ShowBenefitsClickedDelegate();

		public ShowBenefitsClickedDelegate ShowBenefitsClicked;

		public Button ShowBenefitsButton;

		public AudioClip ButtonClickSound;

		private void Start()
		{
			ShowBenefitsButton.onClick.AddListener(delegate
			{
				GetComponent<AudioSource>().PlayOneShot(ButtonClickSound);
				if (ShowBenefitsClicked != null)
				{
					ShowBenefitsClicked();
				}
			});
		}
	}
}
