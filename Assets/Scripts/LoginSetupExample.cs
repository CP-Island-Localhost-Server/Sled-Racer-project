using DevonLocalization.Core;
using Disney.DMOAnalytics;
using ErrorPopup.Core;
using UnityEngine;

public class LoginSetupExample : MonoBehaviour
{
	public Language Language = Language.en_US;

	public string LocalizedFilesDir = "Assets/Framework/Login/Resources/Translations";

	public string errorsJsonFile = "Assets/Framework/Login/Resources/ErrorPopup/LoginErrors.json.txt";

	public string DMOAnalyticsKey;

	public string DMOAnalyticsSecret;

	public string ApplicationBundleIdentifer;

	public string ApplicationVersion;

	private void Awake()
	{
		DMOAnalytics.SharedAnalytics.initWithAnalyticsKeySerect(this, DMOAnalyticsKey, DMOAnalyticsSecret);
	}

	private void Start()
	{
		SetupLocalization();
		SetupErrorPopups();
		SetupAnalytics();
	}

	private void SetupLocalization()
	{
		Localizer.Instance.Language = Language;
		ILocalizedTokenFilePath path = new ModuleTokensFilePath(LocalizedFilesDir, "login", Platform.global);
		Localizer.Instance.LoadTokensFromLocalJSON(path, onTokensLoaded);
	}

	private void onTokensLoaded(bool tokensUpdated)
	{
		UnityEngine.Debug.Log("Tokens successfully loaded.");
	}

	private void SetupErrorPopups()
	{
		ErrorsMap.Instance.LoadErrorsLocally(errorsJsonFile);
	}

	private void SetupAnalytics()
	{
		DMOAnalytics.SharedAnalytics.LogAppStart();
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			DMOAnalytics.SharedAnalytics.LogAppBackground();
		}
		else
		{
			DMOAnalytics.SharedAnalytics.LogAppForeground();
		}
	}

	private void OnApplicationQuit()
	{
		DMOAnalytics.SharedAnalytics.LogAppEnd();
	}
}
