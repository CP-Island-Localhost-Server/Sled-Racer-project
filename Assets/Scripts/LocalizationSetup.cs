using DevonLocalization.Core;
using UnityEngine;

public class LocalizationSetup : MonoBehaviour
{
	public Language Language = Language.en_US;

	public string ModuleId = string.Empty;

	public string LocalizedFilesDir = "Assets/Framework/DevonLocalization/Resources/Translations";

	private void Awake()
	{
		Localizer.Instance.Language = Language;
		ILocalizedTokenFilePath path = (!(ModuleId != string.Empty)) ? ((ILocalizedTokenFilePath)new AppTokensFilePath(LocalizedFilesDir)) : ((ILocalizedTokenFilePath)new ModuleTokensFilePath(LocalizedFilesDir, ModuleId, Platform.global));
		Localizer.Instance.LoadTokensFromLocalJSON(path, onTokensLoaded);
	}

	private void onTokensLoaded(bool tokensUpdated)
	{
		UnityEngine.Debug.Log("Tokens successfully loaded.");
	}
}
