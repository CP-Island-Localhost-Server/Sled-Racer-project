using Disney.DMOAnalytics.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.DMOAnalytics
{
	public class DMOAnalytics
	{
		private static DMOAnalytics mInstance;

		private IDMOAnalyticsBinder mPlatformBinder;

		public string AnalyticsKey
		{
			get;
			private set;
		}

		public string AnalyticsSecret
		{
			get;
			private set;
		}

		public bool DebugLogging
		{
			get
			{
				return DMOAnalyticsHelper.isDebugEnvLog;
			}
			set
			{
				DMOAnalyticsHelper.isDebugEnvLog = value;
				mPlatformBinder.SetDebugLogging(value);
			}
		}

		public bool CanUseNetwork
		{
			get
			{
				return DMOAnalyticsHelper.ICanUseNetwork;
			}
			set
			{
				DMOAnalyticsHelper.ICanUseNetwork = value;
				mPlatformBinder.SetCanUseNetwork(value);
			}
		}

		public static DMOAnalytics SharedAnalytics
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = new DMOAnalytics();
				}
				return mInstance;
			}
		}

		private DMOAnalytics()
		{
			mPlatformBinder = new DMOBinderAndroid();
		}

		public static string GetLibVersion()
		{
			return DMOAnalyticsSysInfo.mVersion;
		}

		public void initWithAnalyticsKeySerect(MonoBehaviour GameObj, string AppKey, string AppSecret)
		{
			initWithAnalyticsKeySecretAppInfo(GameObj, AppKey, AppSecret, "com.unity.player", "1.0");
		}

		public void initWithAnalyticsKeySecretAppInfo(MonoBehaviour gameObj, string appKey, string appSecret, string bundleIdentifier, string appVersion)
		{
			if (gameObj == null || string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(appSecret) || string.IsNullOrEmpty(bundleIdentifier) || string.IsNullOrEmpty(appVersion))
			{
				throw new ArgumentException("Please make sure you have the correct Analytics Key,  Analytics Secret, BundleIdentifier and AppVersion for DMOAnalytics");
			}
			AnalyticsKey = appKey;
			AnalyticsSecret = appSecret;
			DMOAnalyticsSysInfo.setAppVersion(appVersion);
			DMOAnalyticsSysInfo.setBundelIdentifer(bundleIdentifier);
			//mPlatformBinder.Init(gameObj, AnalyticsKey, AnalyticsSecret);
		}

		public void LogEvent(string eventInfo)
		{
			if (eventInfo != null)
			{
				DMOAnalyticsHelper.Log("Log Event: " + eventInfo);
			//	mPlatformBinder.LogEvent(eventInfo);
			}
		}

		public void LogAppStart()
		{
			DMOAnalyticsHelper.Log("Log app_start ");
			//mPlatformBinder.LogAppStart();
		}

		public void LogAppForeground()
		{
			DMOAnalyticsHelper.Log("Log app_foreground ");
			//mPlatformBinder.LogAppForeground();
		}

		public void LogAppBackground()
		{
			DMOAnalyticsHelper.Log("Log app_background ");
			//mPlatformBinder.LogAppBackground();
		}

		public void LogAppEnd()
		{
			DMOAnalyticsHelper.Log("Log app_end");
			//mPlatformBinder.LogAppEnd();
		}

		public void LogGameAction(Dictionary<string, object> gameData)
		{
			if (gameData != null)
			{
				DMOAnalyticsHelper.Log("Log game_action");
			//	mPlatformBinder.LogGameAction(gameData);
			}
		}

		public void LogMoneyAction(Dictionary<string, object> moneyData)
		{
			if (moneyData != null)
			{
				DMOAnalyticsHelper.Log("Log money action");
			//	mPlatformBinder.LogMoneyAction(moneyData);
			}
		}

		public void LogAnalyticsEventWithContext(string eventName, Dictionary<string, object> dataDetails)
		{
			if (eventName != null && dataDetails != null)
			{
				DMOAnalyticsHelper.Log("Log arbitrary action with Context: " + eventName);
			//	mPlatformBinder.LogEventWithContext(eventName, dataDetails);
			}
		}

		public void FlushAnalyticsQueue()
		{
			//	mPlatformBinder.FlushAnalyticsQueue();
		}
	}
}
