using DevonLocalization.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguagesPanelBehaviour : MonoBehaviour
{
	public delegate void OnLanguageChangedDelegate(Language language);

	public GameObject LanguageTogglePrefab;

	public OnLanguageChangedDelegate OnLanguageChanged;

	private void Start()
	{
		foreach (int value in Enum.GetValues(typeof(Language)))
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(LanguageTogglePrefab);
			gameObject.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), worldPositionStays: false);
			gameObject.GetComponent<LanguageToggleBehaviour>().Language = (Language)value;
			if (Localizer.Instance.Language == (Language)value)
			{
				gameObject.GetComponent<Toggle>().isOn = true;
			}
			gameObject.GetComponent<Toggle>().group = GetComponent<ToggleGroup>();
			LanguageToggleBehaviour component = gameObject.GetComponent<LanguageToggleBehaviour>();
			component.OnToggleClicked = (LanguageToggleBehaviour.OnToggleClickedDelegate)Delegate.Combine(component.OnToggleClicked, new LanguageToggleBehaviour.OnToggleClickedDelegate(OnLangToggleClicked));
			gameObject.GetComponentInChildren<Text>().text = LocalizationLanguage.GetLanguageString((Language)value);
		}
	}

	private void OnLangToggleClicked(Language language)
	{
		if (OnLanguageChanged != null)
		{
			OnLanguageChanged(language);
		}
	}
}
