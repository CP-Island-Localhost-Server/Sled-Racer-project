using DevonLocalization.Core;
using Disney.ClubPenguin.ParentPermission;
using Disney.DMOAnalytics;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.ForcedUpdate
{
	public class ForcedUpdateController : MonoBehaviour
	{
		public RectTransform DefaultBackground;

		public Button CloseButton;

		public Text TitleText;

		public Button OkButton;

		public ParentPermissionController parentPermissionPrefab;

		private ParentPermissionController parentPermissionInstance;

		private Action<string> parentPermissionCallbackFunction;

		private string parentPermissionURL;

		private Image modalImage;

		public long? PlayerId
		{
			get;
			set;
		}

		public string AppStoreURL
		{
			get;
			set;
		}

		private void Awake()
		{
			modalImage = GetComponent<Image>();
			SetupLocalization();
		}

		private void SetupLocalization()
		{
			string moduleId = "FORCEDUPDATE";
			string directoryPath = "Resources/Translations";
			ILocalizedTokenFilePath path = new ModuleTokensFilePath(directoryPath, moduleId, Platform.global);
			Localizer.Instance.LoadTokensFromLocalJSON(path, onTokensLoaded);
		}

		private void onTokensLoaded(bool tokensUpdated)
		{
			if (tokensUpdated)
			{
				UnityEngine.Debug.Log("ForcedUpdate tokens successfully loaded.");
			}
			else
			{
				UnityEngine.Debug.LogWarning("Failed to load ForcedUpdate tokens. Default translations will be shown.");
			}
		}

		private void Start()
		{
			if (CloseButton != null)
			{
				CloseButton.onClick.AddListener(delegate
				{
					OnCloseButtonpressed();
				});
			}
			OkButton.onClick.AddListener(delegate
			{
				OnOkButtonClicked();
			});
		}

		private void OnCloseButtonpressed()
		{
			base.transform.SetParent(null);
			base.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		private void OnOkButtonClicked()
		{
			if (AppStoreURL != null)
			{
				ShowParentPermisson(Application.OpenURL, AppStoreURL);
			}
		}

		public void ShowParentPermisson(Action<string> callback, string url = "")
		{
			callback(url);
		}

		private void OnHideParentPermission(int age)
		{
			parentPermissionInstance.onFailClose -= OnHideParentPermission;
			parentPermissionInstance.onSuccess -= OnParentPermissionSuccess;
			parentPermissionInstance.onFailClose -= LogParentPermissionBI;
			parentPermissionInstance.onSuccess -= LogParentPermissionBI;
			UnityEngine.Object.Destroy(parentPermissionInstance.gameObject);
		}

		private void LogParentPermissionBI(int age)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("context", "age_gate");
			dictionary.Add("action", "age_gate");
			dictionary.Add("type", age.ToString());
			dictionary.Add("player_id", PlayerId.HasValue ? ((object)PlayerId) : "NULL");
			Disney.DMOAnalytics.DMOAnalytics.SharedAnalytics.LogGameAction(dictionary);
		}

		private void OnParentPermissionSuccess(int age)
		{
			parentPermissionCallbackFunction(parentPermissionURL);
			OnHideParentPermission(age);
		}
	}
}
