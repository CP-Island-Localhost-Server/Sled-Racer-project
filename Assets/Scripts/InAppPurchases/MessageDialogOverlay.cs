using DevonLocalization;
using Disney.ClubPenguin.CPModuleUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	[RequireComponent(typeof(AudioSource))]
	public class MessageDialogOverlay : MonoBehaviour
	{
		public delegate void ClosedDelegate();

		public ClosedDelegate Closed;

		public LocalizedText LocalizedStatusText;

		public Text StatusCode;

		public Button CloseButton;

		public AudioClip OKButtonSound;

		private RectTransform parentContainer;

		public RectTransform ParentContainer
		{
			get
			{
				return parentContainer;
			}
			set
			{
				parentContainer = value;
				GetComponent<RectTransform>().SetParent(ParentContainer, worldPositionStays: false);
				base.gameObject.SetActive(value: false);
			}
		}

		private void Awake()
		{
			CloseButton.onClick.AddListener(delegate
			{
				UnityEngine.Debug.Log("FIRED!~~");
				GetComponent<AudioSource>().PlayOneShot(OKButtonSound);
				StartCoroutine(WaitForSound());
			});
		}

		private IEnumerator WaitForSound()
		{
			yield return null;
			if (Closed != null)
			{
				Closed();
			}
			base.gameObject.SetActive(value: false);
		}

		public void ShowStatusTextFromString(string statusText, string statusCode = null)
		{
			Show();
			LocalizedStatusText.doNotLocalize = true;
			LocalizedStatusText.GetComponent<Text>().text = InputFieldStringUtils.StripQuoteSlashes(statusText);
			if (statusCode != null)
			{
				StatusCode.text = statusCode;
			}
		}

		public void ShowStatusTextFromToken(string statusToken, string statusCode = null)
		{
			Show();
			LocalizedStatusText.doNotLocalize = false;
			LocalizedStatusText.token = statusToken;
			LocalizedStatusText.UpdateToken();
			if (statusCode != null)
			{
				StatusCode.text = statusCode;
			}
		}

		private void Show()
		{
			if (ParentContainer == null)
			{
				UnityEngine.Debug.LogWarning("MessageDialogOverlay does not have a parent container specified.");
				return;
			}
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
			base.gameObject.SetActive(value: true);
		}
	}
}
