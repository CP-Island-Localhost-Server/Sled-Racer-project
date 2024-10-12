using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public class DMOAnalyticsHelper
	{
		public static bool isDebugEnvLog;

		public static bool ICanUseNetwork = true;

		public static bool RestrictedTracking;

		public static void Log(string msg)
		{
			if (isDebugEnvLog)
			{
				UnityEngine.Debug.Log(msg);
			}
		}

		public static string GetStringFromDictionary(Dictionary<string, object> dictData)
		{
			string result = string.Empty;
			if (dictData != null)
			{
				result = JsonMapper.ToJson(dictData);
			}
			return result;
		}
	}
}
