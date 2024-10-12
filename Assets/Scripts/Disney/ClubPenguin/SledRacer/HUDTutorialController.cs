using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class HUDTutorialController : MonoBehaviour
	{
		public GameEvent.Type tutorialProgress;

		public GameEvent.Type EventToCompleteStep1;

		public GameEvent.Type EventToCompleteStep2;

		public GameEvent.Type EventToCompleteStep3;

		public GameObject[] tutorialPanels;

		private GameObject currentPanel;

		private void Awake()
		{
			DisableAllPanels();
			if (SledRacerGameManager.Instance.CurrentGameState != SledRacerGameManager.GameState.GameTutorial)
			{
				base.enabled = false;
			}
			else
			{
				PlayerController.OnGameEvent += GameEventHandler;
			}
		}

		private void DisableAllPanels()
		{
			for (int i = 0; i < tutorialPanels.Length; i++)
			{
				tutorialPanels[i].SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			PlayerController.OnGameEvent -= GameEventHandler;
		}

		private void GameEventHandler(object sender, GameEvent e)
		{
			int intData = e.intData;
			if (e.type == tutorialProgress)
			{
				UnityEngine.Debug.Log("Heard Progress " + intData);
				if ((bool)currentPanel)
				{
					currentPanel.SetActive(value: false);
				}
				if (intData >= 0 && intData < tutorialPanels.Length)
				{
					currentPanel = tutorialPanels[intData];
					currentPanel.SetActive(value: true);
				}
			}
			if ((bool)currentPanel)
			{
				if (e.type == EventToCompleteStep1)
				{
					currentPanel.SetActive(value: false);
				}
				if (e.type == EventToCompleteStep2)
				{
					currentPanel.SetActive(value: false);
				}
				if (e.type == EventToCompleteStep3)
				{
					currentPanel.SetActive(value: false);
				}
			}
		}
	}
}
