using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.DirectoryService;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class EnvironmentControl : MonoBehaviour
	{
		public GameObject ButtonContainer;

		public GameObject ButtonPrefab;

		public AnimationClip Clip;

		private GameObject CurrentEnvironment;

		private Text CurrentEnvironmentLabel;

		private bool opened;

		private void Start()
		{
			base.gameObject.SetActive(value: true);
			ConfigureCurrentEnvironmentButton();
			ConfigureSelectEnvironmentButons();
		}

		private void Update()
		{
		}

		public void TogglePanelState()
		{
			GetComponent<Animator>().SetTrigger("ShowHide");
			RectTransform component = GetComponent<RectTransform>();
			if (opened)
			{
				SetPanelAnchoredPosition(component, component.rect.height);
				opened = false;
			}
			else
			{
				SetPanelAnchoredPosition(component, 0f);
				opened = true;
			}
		}

		private void SetPanelAnchoredPosition(RectTransform rt, float y)
		{
			Vector2 anchoredPosition = rt.anchoredPosition;
			anchoredPosition.y = y;
			rt.anchoredPosition = anchoredPosition;
		}

		private void ConfigureCurrentEnvironmentButton()
		{
			RectTransform component = GetComponent<RectTransform>();
			SetPanelAnchoredPosition(component, component.rect.height);
			opened = false;
			CurrentEnvironment = GameObject.Find(base.gameObject.GetPath() + "/EnvironmentButton");
			Button component2 = CurrentEnvironment.GetComponent<Button>();
			component2.onClick.AddListener(TogglePanelState);
			CurrentEnvironmentLabel = CurrentEnvironment.GetComponentInChildren<Text>();
			CPEnvironment environment = Service.Get<IDirectoryServiceClient>().Environment;
			CurrentEnvironmentLabel.text = "Environment: " + environment.ToString();
		}

		private void ConfigureSelectEnvironmentButons()
		{
			if (ButtonContainer == null)
			{
				UnityEngine.Debug.LogWarning("No Container for Buttons");
				return;
			}
			IEnumerable<CPEnvironment> values = EnumUtil.GetValues<CPEnvironment>();
			foreach (CPEnvironment item in values)
			{
				UnityEngine.Debug.Log(item);
				AddEnvironmentButton(item);
			}
		}

		private void AddEnvironmentButton(CPEnvironment _val)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(ButtonPrefab) as GameObject;
			gameObject.name = "btn_enviro_" + _val.ToString();
			gameObject.transform.SetParent(ButtonContainer.transform, worldPositionStays: false);
			Button component = gameObject.GetComponent<Button>();
			component.onClick.AddListener(delegate
			{
				OnSelectEnvironment(_val);
			});
			Text componentInChildren = gameObject.GetComponentInChildren<Text>();
			componentInChildren.text = _val.ToString();
		}

		private void OnSelectEnvironment(CPEnvironment _val)
		{
			TogglePanelState();
			PlayerPrefs.SetInt("TargetEnvironment", (int)_val);
			PlayerPrefs.Save();
			CurrentEnvironmentLabel.text = "Environment: " + _val.ToString();
		}
	}
}
