using Disney.ClubPenguin.Service.DirectoryService;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.ClubPenguin.SledRacer.Test;
using Disney.ExtendedTestTools.Utils;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer.TestHarness
{
	public class UITestHarnessGameManager : SledRacerGameManager
	{
		public bool CreateMockLeaderBoard = true;

		public bool CreateMockTrackManager = true;

		public bool CreateMockPlayerData = true;

		public bool LoadUIScene = true;

		public string PostLoadMessage = "ShowMainMenu";

		public int LastScore;

		public string Username = "Gooey";

		public string Password = "client";

		private GameObject UIManagerObj;

		public bool IsLoaded
		{
			get;
			set;
		}

		public UITestHarnessGameManager()
		{
			IsLoaded = false;
		}

		public void Start()
		{
			Service.ResetAll();
			UnityEngine.Debug.Log("UITestHarnessGameManager.Start()");
			if (CreateMockLeaderBoard)
			{
				UnityEngine.Debug.Log("[UITestHarnessGameManager] CreateMockLeaderBoard");
				Service.Set((LeaderboardManager)new MockLeaderboardManager());
			}
			else
			{
				Service.Set(new LeaderboardManager());
			}
			if (CreateMockTrackManager)
			{
				TrackManager = new GameObject();
				TrackManager.AddComponent<UITestHarnessTrackManager>();
				UnityEngine.Debug.Log("[UITestHarnessGameManager] Created Mock Track Manager" + (TrackManager != null));
			}
			if (CreateMockPlayerData)
			{
				Service.Set(new PlayerDataService());
				Service.Get<PlayerDataService>().AllowAutoLogin = false;
				Service.Get<PlayerDataService>().PlayerData.LastScore = LastScore;
				Service.Get<PlayerDataService>().PlayerData.hasTrophy = false;
				Service.Get<PlayerDataService>().PlayerData.RewardStatus = LeaderBoardRewardStatus.RewardStatus.NOT_THE_LEADER;
				UnityEngine.Debug.Log("[UITestHarnessGameManager] Created Offline PlayerData");
			}
			IMWSClient instance = MWSClient.Instance;
			Service.Set(instance);
			Service.Set(DirectoryServiceClient.Instance);
			Service.Set(new EventDataService());
			Service.Set(new BoostManager());
			Service.Set((IAudio)new Audio());
			Service.Set((IBILogging)new BILogging());
			if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
			{
				try
				{
					DirectoryServiceClient.Instance.Environment = CPEnvironment.SANDBOX;
					IGetAuthTokenResponse authToken = instance.GetAuthToken("CPMCAPP", "1.6", Username, Password);
					if (authToken.IsError)
					{
						UnityEngine.Debug.LogWarning("[UITestHarnessGameManager] Failed to login in as " + Username);
					}
					else
					{
						IGetAccountResponse account = instance.GetAccount();
						if (account.IsError)
						{
							UnityEngine.Debug.LogWarning("[UITestHarnessGameManager] Failed to get Account for " + Username + ". Player will be treated as offline guest.");
						}
						else
						{
							LeaderBoardRewardStatus leaderBoardRewardStatus = new LeaderBoardRewardStatus();
							leaderBoardRewardStatus.Status = LeaderBoardRewardStatus.RewardStatus.NOT_THE_LEADER;
							Service.Get<PlayerDataService>().setPlayerAccount(account.Account, leaderBoardRewardStatus);
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("[UITestHarnessGameManager] Failed to login in as " + Username);
					throw ex;
					IL_01f9:;
				}
			}
			if (LoadUIScene)
			{
				StartUI();
			}
		}

		private void StartUI()
		{
			UIManagerObj = GameObject.Find("/UIManager");
			IsLoaded = true;
			UIManager = UIManagerObj.GetComponent<UIManager>();
			UIManager.init();
			GameObject gameObject = Utils.FindChildObject(UIManager.gameObject, "LoadingPanel");
			LoadingPanel = gameObject.GetComponent<LoadingPanelController>();
			UIManager.SetLoadingPanel(LoadingPanel);
			Service.Set(UIManager);
			Service.Set(LoadingPanel);
			UnityEngine.Debug.Log("UITestHarness ui = " + UIManager);
			UnityEngine.Debug.Log("UITestHarness PostLoadMessage = " + PostLoadMessage);
			if (PostLoadMessage != null && PostLoadMessage.Length > 0)
			{
				UIManager.SendMessage(PostLoadMessage);
			}
		}
	}
}
