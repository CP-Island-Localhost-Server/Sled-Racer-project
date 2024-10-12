using DevonLocalization.Core;
using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.MoPubAlt;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostMenuController : BaseMenuController
	{
		private const int EQUIPE_SLOTS = 3;

		public GameObject LoadingIcon;

		public Button BackButton;

		public GameObject BoostModulePrefab;

		public GameObject BoostModuleLayout;

		public GameObject BoostSlotPrefab;

		public GameObject BoostSlotLayout;

		public ScrollRectSnapTo BoostScrollSnapTo;

		public Text BoostDescriptionText;

		private EventDataService eventDataService;

		private PlayerDataService playerDataService;

		private BoostManager boostManager;

		private ConfigController config;

		private bool playWasClicked;

		private BoostSlot[] boostSlots = new BoostSlot[3];

		protected override void VStart()
		{
			eventDataService = Service.Get<EventDataService>();
			playerDataService = Service.Get<PlayerDataService>();
			boostManager = Service.Get<BoostManager>();
			config = Service.Get<ConfigController>();
		}

		protected override void Init()
		{
			eventDataService.OnUIEvent += OnUIEvent;
			HardwareBackButtonDispatcher.SetTargetClickHandler(BackButton);
			BoostScrollSnapTo.ElementSelected += OnItemScrollSnap;
			showBoosts();
			if (!playerDataService.PlayerData.Account.Member)
			{
				loadInterstitialAd();
			}
		}

		private void showBoosts()
		{
			BoostPurchaseManager boostPurchaseManager = Service.Get<BoostPurchaseManager>();
			foreach (int value in Enum.GetValues(typeof(BoostManager.AvailableBoosts)))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(BoostModulePrefab) as GameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(BoostModuleLayout.transform);
					gameObject.transform.localScale = Vector3.one;
					BoostSelectItem component = gameObject.GetComponent<BoostSelectItem>();
					component.SetUp((BoostManager.AvailableBoosts)value, ((BoostManager.AvailableBoosts)value).GetTitle(), ((BoostManager.AvailableBoosts)value).GetSprite());
					if (boostPurchaseManager.OwnedBoosts.Contains((BoostManager.AvailableBoosts)value))
					{
						if (boostManager.EquipedBoosts.Contains((BoostManager.AvailableBoosts)value))
						{
							component.Equiped();
						}
						else
						{
							component.Owned();
						}
					}
					else
					{
						component.Unavailable();
					}
				}
			}
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(BoostSlotPrefab) as GameObject;
				if (gameObject2 != null)
				{
					gameObject2.transform.SetParent(BoostSlotLayout.transform);
					gameObject2.transform.SetAsFirstSibling();
					gameObject2.transform.localScale = Vector3.one;
					BoostSlot component2 = gameObject2.GetComponent<BoostSlot>();
					component2.ClearItem();
					boostSlots[3 - i - 1] = component2;
				}
			}
			int num = 0;
			foreach (BoostManager.AvailableBoosts equipedBoost in boostManager.EquipedBoosts)
			{
				if (num >= boostSlots.Length)
				{
					UnityEngine.Debug.LogError("Trying to equipe more boosts than we have slots, EquipeBoosts: " + boostManager.EquipedBoosts.ToString());
				}
				else
				{
					boostSlots[num++].EquipeItem(equipedBoost, equipedBoost.GetTitle(), equipedBoost.GetSprite());
				}
			}
		}

		private void clearBoosts()
		{
			BoostSelectItem[] componentsInChildren = BoostModuleLayout.GetComponentsInChildren<BoostSelectItem>();
			foreach (BoostSelectItem boostSelectItem in componentsInChildren)
			{
				UnityEngine.Object.Destroy(boostSelectItem.gameObject);
			}
			BoostSlot[] componentsInChildren2 = BoostSlotLayout.GetComponentsInChildren<BoostSlot>();
			foreach (BoostSlot boostSlot in componentsInChildren2)
			{
				UnityEngine.Object.Destroy(boostSlot.gameObject);
			}
		}

		private void OnDestroy()
		{
			eventDataService.OnUIEvent -= OnUIEvent;
			HardwareBackButtonDispatcher.ListenForInput = true;
			BoostScrollSnapTo.ElementSelected -= OnItemScrollSnap;
		}

		private void OnItemScrollSnap(GameObject boost)
		{
			BoostDescriptionText.text = boost.GetComponent<BoostSelectItem>().Boost.GetDescription();
		}

		public void PlayGameClicked()
		{
			HardwareBackButtonDispatcher.ListenForInput = false;
			UnityEngine.Debug.Log("PlayerGameClicked");
			Service.Get<LoadingPanelController>().AddLoadingComponent(this, incrementRefCount: false);
			if (!playerDataService.IsPlayerLoggedIn() || Service.Get<LeaderboardManager>().AreFriendHighScoresCached())
			{
				UnityEngine.Debug.Log("Don't need to wait for highscores.");
				SendPlayGame();
			}
			else
			{
				UnityEngine.Debug.Log("Waiting for highscores to load.");
				playWasClicked = true;
				eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.RequestDisablePanelInput));
			}
		}

		private void SendPlayGame()
		{
			eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.Play));
		}

		public void BackButtonClicked()
		{
			eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.MainMenuRequest));
		}

		private void OnUIEvent(object sender, UIEvent e)
		{
			switch (e.type)
			{
			case UIEvent.uiGameEvent.RequestDisablePanelInput:
			case UIEvent.uiGameEvent.RequestEnablePanelInput:
			case UIEvent.uiGameEvent.SelectBoosts:
				break;
			case UIEvent.uiGameEvent.FriendHighScoresLoaded:
				if (playWasClicked)
				{
					eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.RequestEnablePanelInput));
					SendPlayGame();
				}
				break;
			case UIEvent.uiGameEvent.BoostEquiped:
			{
				BoostManager.AvailableBoosts availableBoosts2 = (BoostManager.AvailableBoosts)(int)e.data;
				int num2 = 0;
				while (true)
				{
					if (num2 < boostSlots.Length)
					{
						if (!boostSlots[num2].EquipedBoost.HasValue)
						{
							break;
						}
						num2++;
						continue;
					}
					return;
				}
				boostSlots[num2].EquipeItem(availableBoosts2, availableBoosts2.GetTitle(), availableBoosts2.GetSprite());
				((BoostSelectItem)sender).Equiped();
				boostManager.EquipeBoost(availableBoosts2);
				break;
			}
			case UIEvent.uiGameEvent.BoostUnequiped:
			{
				BoostManager.AvailableBoosts availableBoosts = (BoostManager.AvailableBoosts)(int)e.data;
				BoostSelectItem[] componentsInChildren = BoostModuleLayout.GetComponentsInChildren<BoostSelectItem>();
				int num = 0;
				BoostSelectItem boostSelectItem;
				while (true)
				{
					if (num < componentsInChildren.Length)
					{
						boostSelectItem = componentsInChildren[num];
						if (boostSelectItem.Boost == availableBoosts)
						{
							break;
						}
						num++;
						continue;
					}
					return;
				}
				BoostSlot boostSlot = findSlotWithBoost(availableBoosts);
				if (boostSlot != null)
				{
					boostSlot.ClearItem();
					boostSelectItem.Owned();
					boostManager.UnEquipeBoost(availableBoosts);
				}
				break;
			}
			}
		}

		private BoostSlot findSlotWithBoost(BoostManager.AvailableBoosts boost)
		{
			BoostSlot[] componentsInChildren = BoostSlotLayout.GetComponentsInChildren<BoostSlot>();
			foreach (BoostSlot boostSlot in componentsInChildren)
			{
				if (boostSlot.EquipedBoost == boost)
				{
					return boostSlot;
				}
			}
			return null;
		}

		private void loadInterstitialAd()
		{
			int @int = PlayerPrefs.GetInt("boost.scene.views", 0);
			@int++;
			if (@int % config.MoPubDisplayFrequency == 0)
			{
				UnityEngine.Debug.Log("Showing intersitial ad");
				MoPubAltManager moPubAltManager = new MoPubAltManager();
				moPubAltManager.ShowAd(Service.Get<UIManager>().OverlayContainer.transform, LocalizationLanguage.GetLanguageString(Localizer.Instance.Language).Substring(0, 2));
			}
			PlayerPrefs.SetInt("boost.scene.views", @int);
		}
	}
}
