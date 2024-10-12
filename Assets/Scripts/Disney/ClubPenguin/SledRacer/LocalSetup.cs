using DevonLocalization.Core;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class LocalSetup : MonoBehaviour
	{
		public Language Language = Language.en_US;

		public string TranslationsPath = "Assets/SledRacer/Resources/Translations";

		public string LoginTranslationsPath = "Assets/Framework/Login/Resources/Translations";

		public string IAPTranslationsPath = "Assets/Framework/InAppPurchases/Resources/Translations";

		private void Awake()
		{
			Localizer.Instance.Language = GetLanguage();
			BootLoader.BootLoadComplete += OnBootLoadComplete;
		}

		private void OnDestroy()
		{
			BootLoader.BootLoadComplete -= OnBootLoadComplete;
		}

		private void OnBootLoadComplete()
		{
			ILocalizedTokenFilePath path = new ModuleTokensFilePath(LoginTranslationsPath, "LOGIN", Platform.global);
			Localizer.Instance.LoadTokensFromLocalJSON(path, delegate(bool b)
			{
				OnTokensLoaded(b, LoginTranslationsPath);
			});
			path = new ModuleTokensFilePath(IAPTranslationsPath, "INAPPPURCHASES", Platform.global);
			Localizer.Instance.LoadTokensFromLocalJSON(path, delegate(bool b)
			{
				OnTokensLoaded(b, IAPTranslationsPath);
			});
			path = new AppTokensFilePath(TranslationsPath, Platform.global);
			Localizer.Instance.LoadTokensFromLocalJSON(path, delegate(bool b)
			{
				OnTokensLoaded(b, TranslationsPath);
			});
			path = new AppTokensFilePath(TranslationsPath, Platform.android);
			Localizer.Instance.LoadTokensFromLocalJSON(path, delegate(bool b)
			{
				OnTokensLoaded(b, TranslationsPath);
			});
		}

		private void OnTokensLoaded(bool success, string path)
		{
			if (success)
			{
				UnityEngine.Debug.Log("Tokens successfully loaded for " + Language + " from " + path + ".");
			}
		}

		private Language GetLanguage()
		{
			switch (Application.systemLanguage)
			{
			case SystemLanguage.English:
				return Language.en_US;
			case SystemLanguage.French:
				return Language.fr_FR;
			case SystemLanguage.German:
				return Language.de_DE;
			case SystemLanguage.Spanish:
				return Language.es_LA;
			case SystemLanguage.Portuguese:
				return Language.pt_BR;
			case SystemLanguage.Russian:
				return Language.ru_RU;
			default:
				return Language.en_US;
			}
		}
	}
}
