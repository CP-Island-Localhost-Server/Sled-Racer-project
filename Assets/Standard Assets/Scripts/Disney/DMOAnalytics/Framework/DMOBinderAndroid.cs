using System.Collections.Generic;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public class DMOBinderAndroid : IDMOAnalyticsBinder
	{
		private static AndroidJavaObject _plugin;

		private static AndroidJavaObject playerActivityContext;

		public DMOBinderAndroid()
		{
		}

		public void Init(MonoBehaviour gameObj, string appKey, string appSecret)
		{
		}

		public void LogEvent(string appEvent)
		{
		}

		public void LogAppStart()
		{
		}

		public void LogAppForeground()
		{
		}

		public void LogAppBackground()
		{
		}

		public void LogAppEnd()
		{
		}

		public void LogEventWithContext(string eventName, Dictionary<string, object> data)
		{
		}

		public void FlushAnalyticsQueue()
		{
		}

		public void LogGameAction(Dictionary<string, object> gameData)
		{
		}

		public void LogMoneyAction(Dictionary<string, object> moneyData)
		{
		}

		public void SetDebugLogging(bool isEnable)
		{
		}

		public void SetCanUseNetwork(bool isEnable)
		{
		}
	}
}
