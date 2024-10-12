using Disney.DMOAnalytics;
using System.Collections.Generic;

namespace InAppPurchases.BI
{
	public class SendGameActionBICMD
	{
		private static string EMPTY_PLAYERID = "NULL";

		private static string PLAYERID = "player_id";

		private static string CONTEXT = "context";

		private static string ACTION = "action";

		private static string MESSAGE = "message";

		private static string MESSAGE_SEPERATOR = "=";

		private string contextValue;

		private string actionValue;

		private long playerID;

		private Dictionary<string, string> customParams;

		public SendGameActionBICMD(string contextValue, string actionValue, Dictionary<string, string> customParams = null, long playerID = 0)
		{
			this.contextValue = contextValue;
			this.actionValue = actionValue;
			this.customParams = customParams;
			this.playerID = playerID;
		}

		public void Execute()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(CONTEXT, contextValue);
			dictionary.Add(ACTION, actionValue);
			dictionary.Add(MESSAGE, BuildMessageFromDictionary(customParams));
			if (playerID == 0L)
			{
				dictionary.Add(PLAYERID, EMPTY_PLAYERID);
			}
			else
			{
				dictionary.Add(PLAYERID, playerID);
			}
			DMOAnalytics.SharedAnalytics.LogGameAction(dictionary);
		}

		private string BuildMessageFromDictionary(IDictionary<string, string> parameters)
		{
			if (parameters == null || parameters.Count < 1)
			{
				return string.Empty;
			}
			string text = string.Empty;
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				text = text + parameter.Key + MESSAGE_SEPERATOR + parameter.Value;
			}
			return text;
		}
	}
}
