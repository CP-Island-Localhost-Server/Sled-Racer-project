using Disney.ClubPenguin.Service.DirectoryService;
using strange.extensions.injector.api;
using UnityEngine;

namespace Disney.ClubPenguin.ForcedUpdate
{
	[Implements(typeof(IForcedUpdateManager), InjectionBindingScope.CROSS_CONTEXT)]
	public class ForcedUpdateManager : IForcedUpdateManager
	{
		private const int DEFAULT_VERSION_CHECK_INTERVAL = 60;

		private static IForcedUpdateManager instance = new ForcedUpdateManager();

		private GameObject parentView;

		private bool alertOnUpdateRecommended;

		private long? playerId;

		private string appStoreUrl;

		private IDirectoryServiceClient directoryServiceClient;

		private bool onUpdateListenersRegistered;

		public static IForcedUpdateManager Instance
		{
			get
			{
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public GameObject ParentView
		{
			get
			{
				return parentView;
			}
			set
			{
				parentView = value;
			}
		}

		public bool AlertOnUpdateRecommended
		{
			get
			{
				return alertOnUpdateRecommended;
			}
			set
			{
				alertOnUpdateRecommended = value;
			}
		}

		public long? PlayerId
		{
			get
			{
				return playerId;
			}
			set
			{
				playerId = value;
			}
		}

		[Inject]
		public IDirectoryServiceClient DirectoryServiceClient
		{
			get
			{
				if (directoryServiceClient == null)
				{
					return Disney.ClubPenguin.Service.DirectoryService.DirectoryServiceClient.Instance;
				}
				return directoryServiceClient;
			}
			set
			{
				directoryServiceClient = value;
			}
		}

		public void Init(GameObject parentView, string appId, string appVersion, string appStoreUrl)
		{
			Init(parentView, appId, appVersion, appStoreUrl, 60);
		}

		public void Init(GameObject parentView, string appId, string appVersion, string appStoreUrl, int versionCheckInterval)
		{
			this.parentView = parentView;
			DirectoryServiceClient.AppID = appId;
			DirectoryServiceClient.AppVersion = appVersion;
			DirectoryServiceClient.VersionCheckInterval = versionCheckInterval;
			this.appStoreUrl = appStoreUrl;
			if (!onUpdateListenersRegistered)
			{
				DirectoryServiceClient.OnUpdateRecommended += OnUpdateRecommended;
				DirectoryServiceClient.OnUpdateRequired += OnUpdateRequired;
				onUpdateListenersRegistered = true;
			}
		}

		public void CheckVersion()
		{
			DirectoryServiceClient.CheckVersion();
		}

		public void OnUpdateRecommended()
		{
			if (alertOnUpdateRecommended)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("RecommendUpdatePanel") as GameObject);
				gameObject.transform.SetParent(ParentView.transform, worldPositionStays: false);
				ForcedUpdateController component = gameObject.GetComponent<ForcedUpdateController>();
				component.AppStoreURL = appStoreUrl;
				component.PlayerId = playerId;
			}
		}

		public void OnUpdateRequired()
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("RequireUpdatePanel") as GameObject);
			gameObject.transform.SetParent(ParentView.transform, worldPositionStays: false);
			ForcedUpdateController component = gameObject.GetComponent<ForcedUpdateController>();
			component.AppStoreURL = appStoreUrl;
			component.PlayerId = playerId;
		}
	}
}
