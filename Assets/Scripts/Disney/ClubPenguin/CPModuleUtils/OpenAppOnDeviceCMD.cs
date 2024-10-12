using System;
using UnityEngine;

namespace Disney.ClubPenguin.CPModuleUtils
{
	public class OpenAppOnDeviceCMD
	{
		public enum APP_OPEN_OUTCOME
		{
			APP,
			URL,
			FAIL
		}

		private const string UNITY_PLAYER_CLASS_NAME = "com.unity3d.player.UnityPlayer";

		private const string ANDROID_CURRENT_ACTIVITY = "currentActivity";

		private const string ANDROID_PACKAGE_MANAGER = "getPackageManager";

		private const string ANDROID_LAUNCH_INTENT = "getLaunchIntentForPackage";

		private const string ANDROID_START_ACTIVITY = "startActivity";

		private string appBundleId;

		private string storeURL;

		private Action<APP_OPEN_OUTCOME> responseHandler;

		public OpenAppOnDeviceCMD(string appBundleId, string storeURL, Action<APP_OPEN_OUTCOME> responseHandler = null)
		{
			this.appBundleId = appBundleId;
			this.storeURL = storeURL;
			this.responseHandler = responseHandler;
		}

		public void Execute()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Application.OpenURL(storeURL);
				if (responseHandler != null)
				{
					responseHandler(APP_OPEN_OUTCOME.URL);
				}
			}
			else if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				UnityEngine.Debug.LogWarning("This class is meant to only run on Android or IOS.");
				if (responseHandler != null)
				{
					responseHandler(APP_OPEN_OUTCOME.FAIL);
				}
			}
		}
	}
}
