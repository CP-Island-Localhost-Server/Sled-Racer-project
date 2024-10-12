using DevonLocalization;
using Disney.ClubPenguin.CPModuleUtils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(AudioSource))]
	public class ConfirmationDialog : MonoBehaviour
	{
		public LocalizedText LocalizedStatusText;

		public Text StatusCode;

		public Button CancelButton;

		public Button OkayButton;

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

		public event Action<bool> Closed;

		private void Awake()
		{
			CancelButton.onClick.AddListener(delegate
			{
				GetComponent<AudioSource>().PlayOneShot(ButtonSound);
				StartCoroutine(WaitForSound(confirmed: false));
			});
			HardwareBackButtonDispatcher.SetTargetClickHandler(CancelButton);
			OkayButton.onClick.AddListener(delegate
			{
				GetComponent<AudioSource>().PlayOneShot(ButtonSound);
				StartCoroutine(WaitForSound(confirmed: true));
			});
		}

		private IEnumerator WaitForSound(bool confirmed)
		{
			yield return null;
			if (this.Closed != null)
			{
				this.Closed(confirmed);
				this.Closed = null;
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
				UnityEngine.Debug.LogWarning("ConfirmationDialog does not have a parent container specified.");
			}
			else
			{
				base.gameObject.SetActive(value: true);
			}
		}
	}
}
