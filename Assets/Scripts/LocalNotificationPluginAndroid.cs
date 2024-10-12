using System;
using UnityEngine;

public class LocalNotificationPluginAndroid : LocalNotificationPlugin
{
	private const string NOTIFICATION_ID_KEY = "plugin.localnotification.scheduled.notifications";

	public override void scheduleLocalNotification(int seconds, string bodyText, string actionText)
	{
		SaveNotificationID(EtceteraAndroid.scheduleNotification(seconds, actionText, bodyText, bodyText, string.Empty, "ic_stat_largenotify", string.Empty));
	}

	private void SaveNotificationID(int notificationID)
	{
		string text = PlayerPrefs.GetString("plugin.localnotification.scheduled.notifications", string.Empty);
		if (!string.IsNullOrEmpty(text))
		{
			text += ",";
		}
		text += notificationID.ToString();
		PlayerPrefs.SetString("plugin.localnotification.scheduled.notifications", text);
	}

	public override void cancelAllNotifications()
	{
		string @string = PlayerPrefs.GetString("plugin.localnotification.scheduled.notifications", string.Empty);
		string[] array = @string.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				UnityEngine.Debug.LogWarning("LocalNotificationPluginAndroid.cancelAllNotifications: " + array[i]);
				try
				{
					EtceteraAndroid.cancelNotification(int.Parse(array[i]));
				}
				catch (FormatException)
				{
					UnityEngine.Debug.LogWarning("LocalNotificationPluginAndroid.cancelAllNotifications: Encountered a notification id of invalid format.");
				}
				catch (OverflowException)
				{
					UnityEngine.Debug.LogWarning("LocalNotificationPluginAndroid.cancelAllNotifications: Encountered a notification id over 32 bits.");
				}
			}
		}
		PlayerPrefs.SetString("plugin.localnotification.scheduled.notifications", string.Empty);
	}
}
