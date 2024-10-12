using Disney.DMOAnalytics.Framework;
using System.Collections.Generic;

public class AppIconsController
{
	public enum AppId
	{
		CLUB_PENGUIN,
		SOUND_STUDIO
	}

	public enum IconLocation
	{
		TILE,
		MEMBERSHIP,
		LEADERBOARD,
		ADVERT,
		GOLDEN_ITEM
	}

	public struct TrackedApp
	{
		public AppId id;

		public IconLocation location;
	}

	public class AppConfig
	{
		public AppId AppId;

		public string AndroidAppBundleId;

		public string AndroidAppStoreURL;

		public string AndroidAppStoreTrackingURL;

		public string AppleAppBundleId;

		public string AppleAppStoreURL;

		public string AppleAppStoreTrackingURL;

		public string IconImage;

		public string GetAppleAppStoreTrackingURL(IconLocation iconLocation)
		{
			return AppleAppStoreTrackingURL + iconLocation.ToString().ToLower() + "&device_id=" + DMOAnalyticsSysInfo._getUniqueIdentifier();
		}

		public string GetAndroidAppStoreTrackingURL(IconLocation iconLocation)
		{
			return AndroidAppStoreTrackingURL + iconLocation.ToString().ToLower() + "&device_id=" + DMOAnalyticsSysInfo._getUniqueIdentifier();
		}
	}

	private static IDictionary<AppId, AppConfig> appIcons;

	public static IDictionary<AppId, AppConfig> AppIcons
	{
		get
		{
			if (appIcons == null)
			{
				getAppIcons();
			}
			return appIcons;
		}
		private set
		{
		}
	}

	private static void getAppIcons()
	{
		appIcons = new Dictionary<AppId, AppConfig>();
		AppConfig appConfig = new AppConfig();
		appConfig.AppId = AppId.CLUB_PENGUIN;
		appConfig.AndroidAppBundleId = "com.disney.cpcompanion_goo";
		appConfig.AndroidAppStoreURL = "https://play.google.com/store/apps/details?id=com.disney.cpcompanion_goo";
		appConfig.AndroidAppStoreTrackingURL = "https://control.kochava.com/v1/cpi/click?campaign_id=koclubpenguingoogleandroid1560528e84a21bcb84f5be564eceb9&network_id=468&site_id=cpsledrace_an-";
		appConfig.AppleAppBundleId = "clubpenguin://";
		appConfig.AppleAppStoreURL = "http://itunes.apple.com/app/id505544063";
		appConfig.AppleAppStoreTrackingURL = "https://control.kochava.com/v1/cpi/click?campaign_id=komypenguinios1066521e22302b0f97b622a93cfd31&network_id=468&site_id=cpsledrace_ip-";
		appConfig.IconImage = "SharedAssets_ClubPengiunBTN";
		AppConfig appConfig2 = new AppConfig();
		appConfig2.AppId = AppId.SOUND_STUDIO;
		appConfig2.AndroidAppBundleId = "com.disney.clubpenguinsoundstudio_goo";
		appConfig2.AndroidAppStoreURL = "https://play.google.com/store/apps/details?id=com.disney.clubpenguinsoundstudio_goo";
		appConfig2.AndroidAppStoreTrackingURL = "https://control.kochava.com/v1/cpi/click?campaign_id=koclub-penguin-sound-studio-google54ad8b1f31c9d16a8e4c280cd1&network_id=468&append_app_conv_trk_params=1&site_id=cpsledrace_an-";
		appConfig2.AppleAppBundleId = "ha1e3d4401c074850da8d983d3044d482d://";
		appConfig2.AppleAppStoreURL = "http://itunes.apple.com/app/id945905982";
		appConfig2.AppleAppStoreTrackingURL = "https://control.kochava.com/v1/cpi/click?campaign_id=koclub-penguin-sound-studio-ios54ad8a856be26cf12905dc2f07&network_id=468&site_id=cpsledrace_ip-";
		appConfig2.IconImage = "SharedAssets_SoundStudioBTN";
		appIcons.Add(appConfig.AppId, appConfig);
	}
}
