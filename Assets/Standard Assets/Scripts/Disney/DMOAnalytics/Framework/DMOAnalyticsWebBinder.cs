using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public class DMOAnalyticsWebBinder : IDMOAnalyticsBinder
	{
		private IDMONetworkRequest mNetworking;

		public DMOAnalyticsWebBinder(IDMONetworkRequest networking)
		{
			if (networking == null)
			{
				throw new ArgumentNullException("networking");
			}
			mNetworking = networking;
		}

		public void Init(MonoBehaviour gameObj, string appID, string appKey)
		{
			mNetworking.GameObj = gameObj;
		}

		public void LogEvent(string eventName)
		{
			Dictionary<string, object> coreEventInfo = DMOAnalyticsSysInfo.GetCoreEventInfo();
			coreEventInfo.Add("method", eventName);
			mNetworking.StartCoroutine(coreEventInfo);
		}

		public void LogAppStart()
		{
			Dictionary<string, object> sysInfo = DMOAnalyticsSysInfo.GetSysInfo();
			sysInfo.Add("method", "app_start");
			sysInfo.Add("is_new_user", DMOAnalyticsSysInfo.getIsNewUser());
			sysInfo.Add("tag", "clicked_link");
			mNetworking.StartCoroutine(sysInfo);
		}

		public void LogAppForeground()
		{
			LogAppEvent(DMOAnalyticsSysInfo.appForeground);
		}

		public void LogAppBackground()
		{
			LogAppEvent(DMOAnalyticsSysInfo.appBackground);
		}

		public void LogAppEnd()
		{
			LogAppEvent(DMOAnalyticsSysInfo.appEnd);
		}

		private void LogAppEvent(string eventName)
		{
			Dictionary<string, object> coreEventInfo = DMOAnalyticsSysInfo.GetCoreEventInfo();
			coreEventInfo.Add("method", eventName);
			if (eventName.Equals(DMOAnalyticsSysInfo.appForeground))
			{
				coreEventInfo.Add("is_new_user", DMOAnalyticsSysInfo.getIsNewUser());
				coreEventInfo.Add("tag", "clicked_link");
			}
			mNetworking.StartCoroutine(coreEventInfo);
		}

		public void LogEventWithContext(string eventName, Dictionary<string, object> data)
		{
			string stringFromDictionary = DMOAnalyticsHelper.GetStringFromDictionary(data);
			Dictionary<string, object> coreEventInfo = DMOAnalyticsSysInfo.GetCoreEventInfo();
			coreEventInfo.Add("method", eventName);
			coreEventInfo.Add("data", stringFromDictionary);
			mNetworking.StartCoroutine(coreEventInfo);
		}

		public void FlushAnalyticsQueue()
		{
			if (DMOAnalyticsHelper.ICanUseNetwork)
			{
				mNetworking.FlushQueue();
			}
		}

		public void LogGameAction(Dictionary<string, object> gameData)
		{
			string stringFromDictionary = DMOAnalyticsHelper.GetStringFromDictionary(gameData);
			Dictionary<string, object> coreEventInfo = DMOAnalyticsSysInfo.GetCoreEventInfo();
			coreEventInfo.Add("data", stringFromDictionary);
			coreEventInfo.Add("method", DMOAnalyticsSysInfo.gameAction);
			coreEventInfo.Add("tag", DMOAnalyticsSysInfo.gameAction);
			mNetworking.StartCoroutine(coreEventInfo);
		}

		public void LogMoneyAction(Dictionary<string, object> moneyData)
		{
			string stringFromDictionary = DMOAnalyticsHelper.GetStringFromDictionary(moneyData);
			Dictionary<string, object> coreEventInfo = DMOAnalyticsSysInfo.GetCoreEventInfo();
			coreEventInfo.Add("data", stringFromDictionary);
			coreEventInfo.Add("method", DMOAnalyticsSysInfo.moneyAction);
			coreEventInfo.Add("tag", DMOAnalyticsSysInfo.moneyAction);
			coreEventInfo.Add("source", "CCBILL");
			coreEventInfo.Add("subtype", "CCBILL");
			mNetworking.StartCoroutine(coreEventInfo);
		}

		public void SetDebugLogging(bool debugLogging)
		{
		}

		public void SetCanUseNetwork(bool canUseNetwork)
		{
		}
	}
}
