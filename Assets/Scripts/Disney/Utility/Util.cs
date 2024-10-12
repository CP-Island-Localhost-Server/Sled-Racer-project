using System;
using UnityEngine;

namespace Disney.Utility
{
	public class Util
	{
		public enum LogLevel
		{
			All,
			Warning,
			Exception,
			Error
		}

		public static void Log(object aMessage)
		{
			Debug.Log(aMessage);
		}

		public static void LogError(object aMessage)
		{
			Log(LogLevel.Error, aMessage);
		}

		public static void LogWarning(object aMessage)
		{
			Log(LogLevel.Warning, aMessage);
		}

		public static void LogException(Exception aException)
		{
			Log(LogLevel.Exception, aException);
		}

		public static void Log(LogLevel level, object aMessage)
		{
			switch (level)
			{
			case LogLevel.Error:
				Debug.LogError(aMessage);
				break;
			case LogLevel.Exception:
				Debug.LogException((Exception)aMessage);
				break;
			case LogLevel.Warning:
				Debug.LogWarning(aMessage);
				break;
			default:
				Debug.Log(aMessage);
				break;
			}
		}
	}
}
