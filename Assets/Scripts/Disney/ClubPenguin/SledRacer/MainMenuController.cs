using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS.Domain;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
    public class MainMenuController : BaseMenuController
    {
        public GameObject AppIconContainer;
        public GameObject AppIconPrefab;
        public GameObject LoginButtonContainer;
        public GameObject LoginButton;
        public GameObject Avatar;
        public GameObject AvatarImageObject;
        public GameObject AvatarHatImageObject;
        public Text PenguinName;
        public Text ClickToLoginText;
        public GameObject LoadingIcon;
        public Button QuitButton;

        private Image avatarImage;
        private Image avatarHatImage;
        private bool autoLoginActive;
        private PlayerDataService playerDataService;
        private EventDataService eventDataService;

        protected override void VStart()
        {
            ShowLoginButton(showText: false, showLoadingIcon: false);
            playerDataService = Service.Get<PlayerDataService>();
            eventDataService = Service.Get<EventDataService>();
            avatarImage = AvatarImageObject.GetComponent<Image>();
            avatarHatImage = AvatarHatImageObject.GetComponent<Image>();
            avatarHatImage.gameObject.SetActive(value: false);
            playerDataService.Subscribe(OnPlayerDataUpdate);
            HardwareBackButtonDispatcher.SetTargetClickHandler(QuitButton);
        }

        [Conditional("FALSE")]
        private void SetupEnvironmentSelector()
        {
            GameObject original = Resources.Load<GameObject>("Prefabs/EnvironmentSelector");
            GameObject gameObject = Object.Instantiate(original) as GameObject;
            gameObject.name = "EnvironmentSelector";
            gameObject.transform.SetParent(base.transform, worldPositionStays: false);
        }

        private void OnDestroy()
        {
            playerDataService.UnSubscribe(OnPlayerDataUpdate);
            eventDataService.OnUIEvent -= OnUIEvent;
        }

        protected override void Init()
        {
            VideoCheck();
            eventDataService.OnUIEvent += OnUIEvent;
            Service.Get<IAudio>().Music.Play(MusicTrack.MainMenu);
            Service.Get<IAudio>().Ambience.Play(AmbienceTrack.UIAmbientGusting);
            Service.Get<IAudio>().Ambience.Volume = 1f;
            ShowAppIcons();
            ShowFirstBootMessage();
            bool flag = playerDataService.IsPlayerLoggedIn();
            if (playerDataService.AllowAutoLogin && !flag && !playerDataService.LoadingAccount)
            {
                playerDataService.AllowAutoLogin = false;
                autoLoginActive = true;
                ShowLoginButton(showText: false, showLoadingIcon: true);
                SledRacerGameManager.Instance.UIManager.TryAutoLogin();
            }
            else if (playerDataService.LoadingAccount || !flag)
            {
                ShowLoginButton(!playerDataService.LoadingAccount, playerDataService.LoadingAccount);
            }
            else
            {
                ShowAvatar();
            }
            Service.Get<IBILogging>().DisneyRefferalStoreIconShown();
        }

        private void VideoCheck()
        {
            bool showVideo = SledRacerGameManager.Instance.showVideo;
            SledRacerGameManager.Instance.showVideo = false;
            if (showVideo)
            {
                string str = "AppIntro-960x540_";
                str += "android.mp4";

#if UNITY_ANDROID
                UnityEngine.Debug.Log($"PlayFullScreenMovie({str}, {Color.black}, {FullScreenMovieControlMode.CancelOnInput}, {FullScreenMovieScalingMode.AspectFill})");
                Handheld.PlayFullScreenMovie(str, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFill);
#else
                UnityEngine.Debug.Log("Full-screen movie playback is only supported on Android. Skipping for this platform.");
#endif
            }
        }

        private void ShowFirstBootMessage()
        {
            if (!PlayerPrefs.HasKey("iap.confirmed"))
            {
                SledRacerGameManager.Instance.UIManager.ShowDialog("firstload.notification.google_play", delegate
                {
                    PlayerPrefs.SetInt("iap.confirmed", 1);
                });
            }
        }

        public void PlayGameClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.SelectBoosts));
        }

        public void LeaderboardClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.LeaderboardRequest));
        }

        public void StoreClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.IAPRequest));
        }

        public void AvatarClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.LoginRequest));
        }

        public void AboutMembershipClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.AboutMembershipRequest));
        }

        public void SettingsClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.SettingsRequest));
        }

        public void DisneyIconClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.DisneyAffiliateRequest));
        }

        public void LogoutClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.Logout));
        }

        public void SwitchPenguinClicked()
        {
            eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.LoginRequest));
        }

        public void QuitClicked()
        {
            UnityEngine.Debug.Log("Application Quit");
            Application.Quit();
        }

        private void ShowAppIcons()
        {
            foreach (AppIconsController.AppConfig value in AppIconsController.AppIcons.Values)
            {
                if ((Application.platform != RuntimePlatform.IPhonePlayer || (value.AppleAppBundleId != null && value.AppleAppStoreURL != null)) &&
                    (Application.platform != RuntimePlatform.Android || (value.AndroidAppBundleId != null && value.AndroidAppStoreURL != null)))
                {
                    GameObject gameObject = Object.Instantiate(AppIconPrefab) as GameObject;
                    AppIconsController.TrackedApp trackedApp;
                    trackedApp.id = value.AppId;
                    trackedApp.location = AppIconsController.IconLocation.TILE;
                    gameObject.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        eventDataService.SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.AffiliateRequest, trackedApp));
                    });
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("AppIcons/" + value.IconImage);
                    gameObject.transform.SetParent(AppIconContainer.transform);
                }
            }
        }

        private void OnUIEvent(object sender, UIEvent e)
        {
            switch (e.type)
            {
                case UIEvent.uiGameEvent.LoginCancelled:
                    break;
                case UIEvent.uiGameEvent.LoginSuccess:
                    OnLoginSuccess();
                    break;
                case UIEvent.uiGameEvent.LoginFailed:
                    OnLoginFailed();
                    break;
                case UIEvent.uiGameEvent.Logout:
                    OnLogout();
                    break;
            }
        }

        private void OnLoginSuccess()
        {
            ShowLoginButton(showText: false, showLoadingIcon: true);
        }

        private void OnPlayerDataUpdate()
        {
            if (playerDataService.IsPlayerLoggedIn())
            {
                UnityEngine.Debug.Log("OnPlayerDataUpdate: RewardStatus=" + playerDataService.PlayerData.RewardStatus.ToString());
                ShowAvatar();
                if (playerDataService.PlayerData.RewardStatus == LeaderBoardRewardStatus.RewardStatus.LEADER_REWARD_GRANTED)
                {
                    Service.Get<UIManager>().ShowGoldenGogglesDialog();
                    playerDataService.PlayerData.RewardStatus = LeaderBoardRewardStatus.RewardStatus.NOT_THE_LEADER_REWARD_OWNED;
                }
            }
        }

        private void OnLoginFailed()
        {
            if (autoLoginActive)
            {
                ShowLoginButton(showText: true, showLoadingIcon: false);
                LoadingIcon.SetActive(value: false);
            }
        }

        private void OnLogout()
        {
            ShowLoginButton(showText: true, showLoadingIcon: false);
            PenguinName.text = string.Empty;
        }

        private void ShowLoginButton(bool showText, bool showLoadingIcon)
        {
            UnityEngine.Debug.Log("[MinMenuController] ShowLoginButton()");
            Avatar.SetActive(value: false);
            LoginButtonContainer.SetActive(value: true);
            ClickToLoginText.enabled = showText;
            LoadingIcon.SetActive(showLoadingIcon);
        }

        private void ShowAvatar()
        {
            UnityEngine.Debug.Log("[MinMenuController] ShowAvatar()");
            LoginButtonContainer.SetActive(value: false);
            Avatar.SetActive(value: true);
            PlayerData playerData = playerDataService.PlayerData;
            if (avatarImage.sprite != null)
            {
                Resources.UnloadAsset(avatarImage.sprite);
            }
            avatarImage.sprite = AvatarUtil.GetLargeAvatar(playerData.Account.Colour);
            if (playerData.hasTrophy)
            {
                avatarHatImage.sprite = Resources.Load<Sprite>("AvatarSprites/Pengiun_GoldHat");
                avatarHatImage.gameObject.SetActive(value: true);
            }
            else
            {
                if (avatarHatImage.sprite != null)
                {
                    Resources.UnloadAsset(avatarHatImage.sprite);
                }
                avatarHatImage.gameObject.SetActive(value: false);
                avatarHatImage.sprite = null;
            }
            PenguinName.text = (!playerDataService.IsPlayerLoggedIn()) ? string.Empty : playerData.Account.Username;
            LoadingIcon.SetActive(value: false);
        }
    }
}
