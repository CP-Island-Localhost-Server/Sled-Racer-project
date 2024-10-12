using DevonLocalization.Core;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class NotificationService
	{
		private const int MEMBER_NOTIFICATION_COUNT = 3;

		private LocalNotificationPlugin plugin;

		private ConfigController config;

		private readonly string[] NOTIFICATION_TOKENS = new string[9]
		{
			"sledracer.notifications.body1",
			"sledracer.notifications.body2",
			"sledracer.notifications.body3",
			"sledracer.notifications.body4",
			"sledracer.notifications.body5",
			"sledracer.notifications.body6",
			"sledracer.notifications.body7",
			"sledracer.notifications.body8",
			"sledracer.notifications.body9"
		};

		public NotificationService()
		{
			plugin = new LocalNotificationPluginAndroid();
			plugin.Initialize();
			config = Service.Get<ConfigController>();
		}

		public void ClearNotifications()
		{
			plugin.cancelAllNotifications();
		}

		public void SetNotifications()
		{
			DateTime now = DateTime.Now;
			string tokenTranslation = Localizer.Instance.GetTokenTranslation("sledracer.notifications.title");
			DateTime dateTime = GetNextWeekday(DateTime.Today, (DayOfWeek)config.NotificationDayOfWeek).AddHours(config.NotificationHourOfDay).AddMinutes(config.NotificationMinuteOfHour);
			if (dateTime.Subtract(now).TotalMinutes < (double)config.NotificationMinFirstNotificationDelayMinutes)
			{
				dateTime = dateTime.AddDays(config.NotificationIntervalDays);
			}
			for (int i = 0; i < config.NotificationNumberOfDelays; i++)
			{
				string tokenTranslation2 = Localizer.Instance.GetTokenTranslation(randomToken());
				plugin.scheduleLocalNotification((int)dateTime.Subtract(now).TotalSeconds, tokenTranslation2, tokenTranslation);
				dateTime = dateTime.AddDays(config.NotificationIntervalDays);
			}
		}

		public string randomToken()
		{
			int max = NOTIFICATION_TOKENS.Length;
			if (Service.Get<PlayerDataService>().PlayerData.Account.Member)
			{
				max = 3;
			}
			return NOTIFICATION_TOKENS[UnityEngine.Random.Range(0, max)];
		}

		public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
		{
			DateTime result = start;
			while (result.DayOfWeek != day)
			{
				result = result.AddDays(1.0);
			}
			return result;
		}
	}
}
