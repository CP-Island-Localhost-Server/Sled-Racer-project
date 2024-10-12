using System.Collections.Generic;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public interface IDMOAnalyticsBinder
	{
		void Init(MonoBehaviour gameObj, string appKey, string appSecret);

		void LogEvent(string appEvent);

		void LogAppStart();

		void LogAppForeground();

		void LogAppBackground();

		void LogAppEnd();

		void LogGameAction(Dictionary<string, object> gameData);

		void LogMoneyAction(Dictionary<string, object> moneyData);

		void LogEventWithContext(string eventName, Dictionary<string, object> data);

		void FlushAnalyticsQueue();

		void SetDebugLogging(bool isEnable);

		void SetCanUseNetwork(bool isEnable);
	}
}
