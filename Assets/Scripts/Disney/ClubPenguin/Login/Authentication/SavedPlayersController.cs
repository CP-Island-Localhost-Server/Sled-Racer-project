using System;
using UnityEngine;

namespace Disney.ClubPenguin.Login.Authentication
{
	public class SavedPlayersController : MonoBehaviour
	{
		public delegate void PlayerSelectedDelegate(SavedPlayerData savedPlayerData);

		public delegate void AddPlayerClickedDelegate();

		public GameObject SavedPlayerPrefab;

		public PlayerSelectedDelegate PlayerSelected;

		public AddPlayerClickedDelegate AddPlayerClicked;

		private SavedPlayerCollection savedPlayersCollection;

		public void SetupSavedPlayers(SavedPlayerCollection value, AudioSource rootAudioSource)
		{
			savedPlayersCollection = value;
			setupSavedPlayers(rootAudioSource);
		}

		private void setupSavedPlayers(AudioSource rootAudioSource)
		{
			for (int i = 0; i < 6; i++)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(SavedPlayerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(GetComponent<RectTransform>(), worldPositionStays: false);
				PlayerSelectionController component2 = gameObject.GetComponent<PlayerSelectionController>();
				component2.RootAudioSource = rootAudioSource;
				if (i < savedPlayersCollection.SavedPlayers.Count)
				{
					SavedPlayerData savedPlayer = savedPlayersCollection.SavedPlayers[i];
					component2.SavedPlayers = savedPlayersCollection;
					component2.SetSavedPlayer(savedPlayer);
					component2.ShowSavedPlayerView();
				}
				else
				{
					component2.ShowAddNewView();
				}
				PlayerSelectionController playerSelectionController = component2;
				playerSelectionController.PlayerSelected = (PlayerSelectionController.PlayerSelectedDelegate)Delegate.Combine(playerSelectionController.PlayerSelected, new PlayerSelectionController.PlayerSelectedDelegate(onPlayerSelected));
				PlayerSelectionController playerSelectionController2 = component2;
				playerSelectionController2.AddPlayerClicked = (PlayerSelectionController.AddPlayerClickedDelegate)Delegate.Combine(playerSelectionController2.AddPlayerClicked, new PlayerSelectionController.AddPlayerClickedDelegate(onAddPlayerClicked));
			}
		}

		private void onPlayerSelected(SavedPlayerData savedPlayerData)
		{
			if (PlayerSelected != null)
			{
				PlayerSelected(savedPlayerData);
			}
		}

		private void onAddPlayerClicked()
		{
			if (AddPlayerClicked != null)
			{
				AddPlayerClicked();
			}
		}
	}
}
