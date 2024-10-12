using DevonLocalization.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class LanguageToggleBehaviour : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public delegate void OnToggleClickedDelegate(Language language);

	public Language Language;

	public OnToggleClickedDelegate OnToggleClicked;

	public void OnPointerClick(PointerEventData eventData)
	{
		Toggle component = GetComponent<Toggle>();
		if (component.isOn)
		{
			UnityEngine.Debug.Log("Language Changed: " + LocalizationLanguage.GetLanguageString(Language));
			if (OnToggleClicked != null)
			{
				OnToggleClicked(Language);
			}
		}
	}
}
