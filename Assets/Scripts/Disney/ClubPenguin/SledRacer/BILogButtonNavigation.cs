using System;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(Button))]
	public class BILogButtonNavigation : MonoBehaviour
	{
		public BIButton ButtonType = BIButton.UNKNOWN;

		public BIScreen CurrentSceen = BIScreen.AUTO_DETECT_SCENE;

		public BIScreen TargetSceen = BIScreen.AUTO_DETECT_SCENE;

		public bool LogAfterParentGate;

		private void OnEnable()
		{
			GetComponent<Button>().onClick.AddListener(logOnClick);
		}

		private void OnDisable()
		{
			GetComponent<Button>().onClick.RemoveListener(logOnClick);
			removeParentGateListeners(this, null);
		}

		private void logOnClick()
		{
			if (LogAfterParentGate)
			{
				Service.Get<UIManager>().OnParentGateSuccess += logAfterParentGate;
				Service.Get<UIManager>().OnParentGateClosed += removeParentGateListeners;
			}
			else
			{
				Log();
			}
		}

		private void removeParentGateListeners(object sender, EventArgs e)
		{
			Service.Get<UIManager>().OnParentGateSuccess -= logAfterParentGate;
			Service.Get<UIManager>().OnParentGateClosed -= removeParentGateListeners;
		}

		private void logAfterParentGate(object sender, EventArgs e)
		{
			if (LogAfterParentGate)
			{
				removeParentGateListeners(this, null);
				Log();
			}
		}

		public void Log()
		{
			BIScreen bIScreen = CurrentSceen;
			if (bIScreen == BIScreen.AUTO_DETECT_SCENE)
			{
				bIScreen = GetScreenFromScene(Service.Get<UIManager>().GetCurrentUIScene());
			}
			BIScreen bIScreen2 = TargetSceen;
			if (bIScreen2 == BIScreen.AUTO_DETECT_SCENE)
			{
				bIScreen2 = GetScreenFromScene(Service.Get<UIManager>().GetTargetUIScene());
			}
			Service.Get<IBILogging>().ButtonPressed(ButtonType, bIScreen, bIScreen2);
		}

		public static BIScreen GetScreenFromScene(string scene)
		{
			UIManager uIManager = Service.Get<UIManager>();
			if (scene.Equals(uIManager.BoostMenuPanel))
			{
				return BIScreen.BOOST_SELECT;
			}
			if (scene.Equals(uIManager.EndGamePanel))
			{
				return BIScreen.END_GAME;
			}
			if (scene.Equals(uIManager.HUDPanel))
			{
				return BIScreen.GAME_HUD;
			}
			if (scene.Equals(uIManager.LeaderBoardPanel))
			{
				return BIScreen.LEADERBOARD;
			}
			if (scene.Equals(uIManager.MainMenuPanel))
			{
				return BIScreen.MAIN_MENU;
			}
			if (scene.Equals(uIManager.PauseMenuPanel))
			{
				return BIScreen.PAUSE;
			}
			if (scene.Equals(uIManager.SettingsPanel))
			{
				return BIScreen.SETTINGS;
			}
			return BIScreen.UNKNOWN;
		}
	}
}
