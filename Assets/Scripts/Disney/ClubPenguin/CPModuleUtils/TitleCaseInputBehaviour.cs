using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.CPModuleUtils
{
	[RequireComponent(typeof(InputField))]
	public class TitleCaseInputBehaviour : MonoBehaviour
	{
		private InputField inputField;

		private string previousInputText = string.Empty;

		private void Awake()
		{
			inputField = GetComponent<InputField>();
		}

		private void Update()
		{
			if (!TouchScreenKeyboard.visible && previousInputText != inputField.text)
			{
				inputField.text = InputFieldStringUtils.ToTitleCase(inputField.text);
				previousInputText = inputField.text;
			}
		}
	}
}
