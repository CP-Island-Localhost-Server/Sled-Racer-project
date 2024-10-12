using UnityEngine;

namespace DevonLocalization.Core
{
	public class LocalizationLanguage
	{
		private static string[] LANGUAGES = new string[7] { "none", "en_US", "pt_BR", "fr_FR", "es_LA", "de_DE", "ru_RU" };

		public static string GetLanguageString(Language langEnum)
		{
			switch (langEnum)
			{
			case Language.none:
				return LANGUAGES[0];
			case Language.en_US:
				return LANGUAGES[1];
			case Language.pt_BR:
				return LANGUAGES[2];
			case Language.fr_FR:
				return LANGUAGES[3];
			case Language.es_LA:
				return LANGUAGES[4];
			case Language.de_DE:
				return LANGUAGES[5];
			case Language.ru_RU:
				return LANGUAGES[6];
			default:
				Debug.LogWarning("Invalid language enumeration: " + (int)langEnum);
				return LANGUAGES[0];
			}
		}
	}
}
