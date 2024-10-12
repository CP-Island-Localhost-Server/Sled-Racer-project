using Disney.DMOAnalytics;
using System;
using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer
{
	public class BILogging : IBILogging
	{
		private const string CONTEXT = "context";

		private const string ACTION = "action";

		private const string MESSAGE = "message";

		private const string PLAYER_ID = "player_id";

		private const string FROM_LOCATION = "from_location";

		private const string TO_LOCATION = "to_location";

		private const string LOCATION = "location";

		private const string BUTTON_PRESSED = "button_pressed";

		private const string PLACEMENT = "placement";

		private const string CREATIVE = "creative";

		private const string TYPE = "type";

		private const string CURRENCY = "currency";

		private const string AMOUNT = "amount";

		private const string SUBTYPE = "subtype";

		private const string ITEM = "item";

		private const string ITEM_ID = "item_id";

		private const string ITEM_COUNT = "item_count";

		private const string LEVEL = "level";

		private const string NAVIGATION_ACTION = "navigation_action";

		private const string AD_ACTION = "ad_action";

		private const string PAGE_VIEW = "page_view";

		private const string IN_APP_CURRENCY_ACTION = "in_app_currency_action";

		public void StartGame(IList<BoostManager.AvailableBoosts> equippedBoosts)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string[] array = new string[equippedBoosts.Count];
			int num = 0;
			foreach (BoostManager.AvailableBoosts equippedBoost in equippedBoosts)
			{
				array[num++] = equippedBoost.ToString().ToLower();
			}
			Array.Sort(array);
			if (array.Length == 0)
			{
				array = new string[1]
				{
					"none"
				};
			}
			dictionary.Add("context", "sled_race_start");
			dictionary.Add("action", string.Join("|", array));
			logGameAction(dictionary);
		}

		public void EndGame(BIGameObjectType killedBy, int finalScore)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("context", "sled_race_end");
			dictionary.Add("action", killedBy.ToString().ToLower());
			dictionary.Add("message", finalScore);
			logGameAction(dictionary);
		}

		public void ButtonPressed(BIButton button, BIScreen startingSceen, BIScreen targetSceen)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("from_location", startingSceen.ToString().ToLower());
			dictionary.Add("to_location", targetSceen.ToString().ToLower());
			dictionary.Add("button_pressed", button.ToString().ToLower());
			addPlayerId(dictionary);
			Disney.DMOAnalytics.DMOAnalytics.SharedAnalytics.LogAnalyticsEventWithContext("navigation_action", dictionary);
		}

		public void BeatFriendsHighScore(bool isTopFriend)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("context", "friend_score_beaten");
			dictionary.Add("action", (!isTopFriend) ? "FALSE" : "TRUE");
			logGameAction(dictionary);
		}

		public void OpenExternalApp(AppIconsController.AppId appId)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("context", "open_external_app");
			dictionary.Add("action", appId.ToString().ToLower());
			logGameAction(dictionary);
		}

		public void AdShown(string adId)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("location", adId);
			addPlayerId(dictionary);
			Disney.DMOAnalytics.DMOAnalytics.SharedAnalytics.LogAnalyticsEventWithContext("page_view", dictionary);
		}

		public void RewardItemGranted(int itemId, string itemType, string itemName)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("currency", "coins");
			dictionary.Add("amount", -1);
			dictionary.Add("type", itemType);
			dictionary.Add("subtype", itemName);
			dictionary.Add("level", -1);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("item_id", itemId);
			dictionary2.Add("item_count", 1);
			dictionary.Add("item", dictionary2);
			addPlayerId(dictionary);
			Disney.DMOAnalytics.DMOAnalytics.SharedAnalytics.LogAnalyticsEventWithContext("in_app_currency_action", dictionary);
		}

		private void logGameAction(Dictionary<string, object> data)
		{
			addPlayerId(data);
			Disney.DMOAnalytics.DMOAnalytics.SharedAnalytics.LogGameAction(data);
		}

		private void addPlayerId(Dictionary<string, object> data)
		{
			PlayerDataService playerDataService = Service.Get<PlayerDataService>();
			data.Add("player_id", (!playerDataService.IsPlayerLoggedIn()) ? "NULL" : ((object)playerDataService.PlayerData.Account.PlayerId));
		}

		public void DisneyRefferalStoreIconShown()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("placement", "More_Disney");
			dictionary.Add("creative", "Main_Button");
			dictionary.Add("type", "Impression");
			Disney.DMOAnalytics.DMOAnalytics.SharedAnalytics.LogAnalyticsEventWithContext("ad_action", dictionary);
		}

		public void ParentGateClosed(int age)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("context", "age_gate");
			dictionary.Add("action", "age_gate");
			dictionary.Add("type", age.ToString());
			logGameAction(dictionary);
		}
	}
}
