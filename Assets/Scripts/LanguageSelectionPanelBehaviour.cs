using DevonLocalization.Core;
using System;
using UnityEngine;

public class LanguageSelectionPanelBehaviour : MonoBehaviour
{
	public LanguagesPanelBehaviour LanguagePanel;

	public string PathToTokens = "Assets/Framework/DevonLocalization/Resources/Translations";

	public string ModuleId = "login";

	public Platform platform;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		LanguagesPanelBehaviour languagePanel = LanguagePanel;
		languagePanel.OnLanguageChanged = (LanguagesPanelBehaviour.OnLanguageChangedDelegate)Delegate.Combine(languagePanel.OnLanguageChanged, new LanguagesPanelBehaviour.OnLanguageChangedDelegate(ChangeLanguage));
	}

	public void TogglePanel()
	{
		LanguagePanel.gameObject.SetActive(!LanguagePanel.gameObject.activeSelf);
	}

	public void ChangeLanguage(Language language)
	{
		Localizer.Instance.ResetTokens();
		Localizer.Instance.Language = language;
		ILocalizedTokenFilePath path = (!(ModuleId != string.Empty)) ? ((ILocalizedTokenFilePath)new AppTokensFilePath(PathToTokens)) : ((ILocalizedTokenFilePath)new ModuleTokensFilePath(PathToTokens, ModuleId, platform));
		Localizer.Instance.LoadTokensFromLocalJSON(path);
	}
}
