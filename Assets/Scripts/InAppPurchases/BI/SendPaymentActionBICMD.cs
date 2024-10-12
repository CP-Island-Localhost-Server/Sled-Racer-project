using Disney.ClubPenguin.CPModuleUtils;
using Disney.DMOAnalytics;
using System.Collections.Generic;

namespace InAppPurchases.BI
{
	public class SendPaymentActionBICMD
	{
		private static string PLAYER_ID = "player_id";

		private static string CURRENCY = "currency";

		private static string LOCALE = "locale";

		private static string AMOUNT_PAID = "amount_paid";

		private static string ITEM_ID = "item_id";

		private static string ITEM_COUNT = "item_count";

		private static string SUBTYPE = "subtype";

		private static string DURABLE = "durable";

		private long playerID;

		private SkuInfo skuInfo;

		private GetDeviceLocaleCMD deviceLocationCMD;

		public SendPaymentActionBICMD(long playerID, SkuInfo skuInfo)
		{
			this.playerID = playerID;
			this.skuInfo = skuInfo;
		}

		public void Execute()
		{
			deviceLocationCMD = new GetDeviceLocaleCMD();
			deviceLocationCMD.LocaleAsString += ReceivedDeviceLocale;
			deviceLocationCMD.Execute();
		}

		private void ReceivedDeviceLocale(string locale)
		{
			deviceLocationCMD.LocaleAsString -= ReceivedDeviceLocale;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PLAYER_ID, playerID);
			dictionary.Add(CURRENCY, skuInfo.currencyCode);
			dictionary.Add(LOCALE, locale);
			dictionary.Add(AMOUNT_PAID, skuInfo.price);
			dictionary.Add(ITEM_ID, skuInfo.sku);
			dictionary.Add(ITEM_COUNT, 1);
			dictionary.Add(SUBTYPE, DURABLE);
			DMOAnalytics.SharedAnalytics.LogAnalyticsEventWithContext("payment_action", dictionary);
		}
	}
}
