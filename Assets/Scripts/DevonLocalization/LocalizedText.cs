using System;
using DevonLocalization.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DevonLocalization
{
	[RequireComponent(typeof(Text))]
	public class LocalizedText : MonoBehaviour
	{
		public bool doNotLocalize;

		public string token = string.Empty;

		public bool stripVisibleNewline = true;

		public string TranslatedText { get; private set; }

		public event Action<string> OnUpdateToken;

		private void Start()
		{
			Localizer instance = Localizer.Instance;
			instance.TokensUpdated = (Localizer.TokensUpdatedDelegate)Delegate.Combine(instance.TokensUpdated, new Localizer.TokensUpdatedDelegate(UpdateToken));
			UpdateToken();
		}

		private void OnDestroy()
		{
			Localizer instance = Localizer.Instance;
			instance.TokensUpdated = (Localizer.TokensUpdatedDelegate)Delegate.Remove(instance.TokensUpdated, new Localizer.TokensUpdatedDelegate(UpdateToken));
		}

		public string StripQuoteSlashes(string input)
		{
			return input.Replace("\\\"", "\"");
		}

		public static string StripVisibleNewLineCharacters(string input)
		{
			bool flag = false;
			if (input.Contains("\\r"))
			{
				input = input.Replace("\\r", " ");
				flag = true;
			}
			if (input.Contains("\\n"))
			{
				input = input.Replace("\\n", (!flag) ? " " : string.Empty);
			}
			return input;
		}

		public void UpdateToken()
		{
			TranslatedText = Localizer.Instance.GetTokenTranslation(token);
			if (this.OnUpdateToken != null)
			{
				this.OnUpdateToken(TranslatedText);
			}
			if (!doNotLocalize)
			{
				GetComponent<Text>().text = StripQuoteSlashes(TranslatedText);
				if (stripVisibleNewline)
				{
					TranslatedText = StripVisibleNewLineCharacters(TranslatedText);
				}
				InputField component = base.transform.parent.GetComponent<InputField>();
				if (component != null)
				{
					component.enabled = false;
					component.enabled = true;
				}
			}
		}
	}
}
