using Disney.ClubPenguin.CPModuleUtils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Disney.ClubPenguin.SledRacer
{
	public class PanelLoader
	{
		private MonoBehaviour owner;

		private GameObject loadedPanel;

		private bool started;

		private bool finished;

		private LoadingPanelController loadingPanel;

		private AsyncOperation loader;

		public bool ShowLoadingScreen
		{
			get;
			private set;
		}

		public string PanelScene
		{
			get;
			private set;
		}

		public bool IsStarted => started;

		public bool IsFinished => finished;

		public AsyncOperation Loader => loader;

		public event Action<GameObject> OnFinished;

		public PanelLoader(string panelScene, MonoBehaviour owner, bool showLoadingScreen, Action<GameObject> finishedCallback = null)
		{
			this.owner = owner;
			PanelScene = panelScene;
			ShowLoadingScreen = showLoadingScreen;
			started = false;
			finished = false;
			loadingPanel = Service.Get<LoadingPanelController>();
			if (string.IsNullOrEmpty(panelScene))
			{
				UnityEngine.Debug.LogError("Attempted to show null or empty panel scene.");
			}
			else if (owner == null)
			{
				UnityEngine.Debug.LogError("owner must not be null.");
			}
			if (finishedCallback != null)
			{
				this.OnFinished = (Action<GameObject>)Delegate.Combine(this.OnFinished, finishedCallback);
			}
		}

		public void Start()
		{
			if (started)
			{
				UnityEngine.Debug.LogError("Panel loader for '" + PanelScene + "' was started more than once.");
				return;
			}
			UnityEngine.Debug.Log("Start loading scene: " + PanelScene + " showLoadingScreen=" + ShowLoadingScreen);
			started = true;
			owner.StartCoroutine(StartAsync());
		}

		private IEnumerator StartAsync()
		{
			if (ShowLoadingScreen)
			{
				loadingPanel.AddLoadingComponent(PanelScene);
			}
			UnityEngine.Debug.Log("Begin LoadLevelAdditiveAsync...");
			loader = SceneManager.LoadSceneAsync(PanelScene, LoadSceneMode.Additive);
			yield return loader;
			UnityEngine.Debug.Log("End LoadLevelAdditiveAsync...");
			loadedPanel = GameObject.Find("/" + PanelScene + " Canvas");
			if (loadedPanel == null)
			{
				UnityEngine.Debug.LogError("Failed to find panel game object for ui scene '" + PanelScene + "' after loading finished.");
				if (ShowLoadingScreen)
				{
					loadingPanel.RemoveLoadingComponent(PanelScene);
				}
				Cleanup();
				yield break;
			}
			if (ShowLoadingScreen)
			{
				Service.Get<EventDataService>().OnUIEvent += OnUIEvent;
			}
			loadedPanel.transform.SetParent(owner.transform);
			UnityEngine.Debug.Log("loaded panel was parented to owner: " + owner.GetPath());
			yield return null;
			finished = true;
			if (this.OnFinished != null)
			{
				this.OnFinished(loadedPanel);
			}
			if (ShowLoadingScreen)
			{
				loadingPanel.RemoveLoadingComponent(PanelScene);
				yield break;
			}
			InitPanel();
			Cleanup();
		}

		private void OnUIEvent(object sender, UIEvent e)
		{
			UIEvent.uiGameEvent type = e.type;
			if (type == UIEvent.uiGameEvent.LoadingComplete)
			{
				UnityEngine.Debug.Log("LoadingComplete");
				InitPanel();
				Cleanup();
			}
		}

		private void InitPanel()
		{
			UnityEngine.Debug.Log("InitPanel: " + loadedPanel.GetPath());
			BaseMenuController[] componentsInChildren = loadedPanel.GetComponentsInChildren<BaseMenuController>();
			foreach (BaseMenuController baseMenuController in componentsInChildren)
			{
				baseMenuController.DoInit();
				baseMenuController.InitAnimations();
			}
		}

		private void Cleanup()
		{
			this.OnFinished = null;
			Service.Get<EventDataService>().OnUIEvent -= OnUIEvent;
		}
	}
}
