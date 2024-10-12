using Disney.DMOAnalytics;
using System.Collections.Generic;

namespace InAppPurchases.BI
{
	public class SendPageViewBICMD
	{
		private const string EMPTY_PLAYERID = "NULL";

		private const string PAGEVIEW = "page_view";

		private const string LOCATION = "location";

		private const string PLAYERID = "player_id";

		private string pageViewName;

		private long playerID;

		public SendPageViewBICMD(string pageViewName, long playerID = 0)
		{
			this.pageViewName = pageViewName;
			this.playerID = playerID;
		}

		public void Execute()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (playerID == 0L)
			{
				dictionary.Add("player_id", "NULL");
			}
			else
			{
				dictionary.Add("player_id", playerID);
			}
			dictionary.Add("location", pageViewName);
			DMOAnalytics.SharedAnalytics.LogAnalyticsEventWithContext("page_view", dictionary);
		}
	}
}
