using DevonLocalization;
using Disney.ClubPenguin.CPModuleUtils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(AudioSource))]
	public class DefaultMessageDialog : MonoBehaviour
	{
		public LocalizedText LocalizedHeaderText;

		public LocalizedText LocalizedMessageText;

		public Button CloseButton;

		public AudioClip ButtonSound;

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

		public event Action Closed;

		private void Awake()
		{
			CloseButton.onClick.AddListener(delegate
			{
				GetComponent<AudioSource>().PlayOneShot(ButtonSound);
				StartCoroutine(WaitForSound());
			});
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
		}

		private IEnumerator WaitForSound()
		{
			yield return null;
			if (this.Closed != null)
			{
				this.Closed();
				this.Closed = null;
			}
			base.gameObject.SetActive(value: false);
		}

		public void ShowStatusTextFromString(string headerText, string statusText)
		{
			Show();
			LocalizedHeaderText.doNotLocalize = true;
			LocalizedHeaderText.GetComponent<Text>().text = InputFieldStringUtils.StripQuoteSlashes(headerText);
			LocalizedMessageText.doNotLocalize = true;
			LocalizedMessageText.GetComponent<Text>().text = InputFieldStringUtils.StripQuoteSlashes(statusText);
		}

		public void ShowStatusTextFromToken(string headerText, string statusToken)
		{
			Show();
			LocalizedHeaderText.doNotLocalize = false;
			LocalizedHeaderText.token = headerText;
			LocalizedHeaderText.UpdateToken();
			LocalizedMessageText.doNotLocalize = false;
			LocalizedMessageText.token = statusToken;
			LocalizedMessageText.UpdateToken();
		}

		private void Show()
		{
			if (ParentContainer == null)
			{
				UnityEngine.Debug.LogWarning("ConfirmationDialog does not have a parent container specified.");
			}
			else
			{
				base.gameObject.SetActive(value: true);
			}
		}
	}
}
