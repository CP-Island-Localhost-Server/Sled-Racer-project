using System;

namespace InAppPurchases
{
	[Serializable]
	public class SavedStorePurchaseData
	{
		public string sku;

		public string price;

		public string currencyCode;

		public bool pendingApproval;

		public bool registeredWithMWS;

		[NonSerialized]
		public string purchaseInfoToken;

		[NonSerialized]
		public string purchaseInfoOrderId;

		public static SavedStorePurchaseData FromCommerceData(PurchaseInfo purchaseInfo, SkuInfo skuInfo, bool pendingApproval, bool registeredWithMWS)
		{
			SavedStorePurchaseData savedStorePurchaseData = new SavedStorePurchaseData();
			savedStorePurchaseData.sku = skuInfo.sku;
			savedStorePurchaseData.price = skuInfo.price;
			savedStorePurchaseData.currencyCode = skuInfo.currencyCode;
			savedStorePurchaseData.purchaseInfoToken = purchaseInfo.token;
			savedStorePurchaseData.purchaseInfoOrderId = purchaseInfo.orderId;
			savedStorePurchaseData.pendingApproval = pendingApproval;
			savedStorePurchaseData.registeredWithMWS = registeredWithMWS;
			return savedStorePurchaseData;
		}

		public PurchaseInfo GetPurchaseInfo()
		{
			return new PurchaseInfo(purchaseInfoOrderId, sku, 0L, purchaseInfoToken, string.Empty);
		}

		public SkuInfo GetSkuInfo()
		{
			return new SkuInfo("title", price, string.Empty, string.Empty, sku, currencyCode, string.Empty);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			SavedStorePurchaseData savedStorePurchaseData = obj as SavedStorePurchaseData;
			if (savedStorePurchaseData == null)
			{
				return false;
			}
			return sku == savedStorePurchaseData.sku && price == savedStorePurchaseData.price && currencyCode == savedStorePurchaseData.currencyCode && purchaseInfoToken == savedStorePurchaseData.purchaseInfoToken && purchaseInfoOrderId == savedStorePurchaseData.purchaseInfoOrderId && pendingApproval == savedStorePurchaseData.pendingApproval && registeredWithMWS == savedStorePurchaseData.registeredWithMWS;
		}

		public bool Equals(SavedStorePurchaseData sspd)
		{
			if (sspd == null)
			{
				return false;
			}
			return sku == sspd.sku && price == sspd.price && currencyCode == sspd.currencyCode && purchaseInfoToken == sspd.purchaseInfoToken && purchaseInfoOrderId == sspd.purchaseInfoOrderId && pendingApproval == sspd.pendingApproval && registeredWithMWS == sspd.registeredWithMWS;
		}

		public override int GetHashCode()
		{
			string text = sku + price + currencyCode + purchaseInfoToken + purchaseInfoOrderId + pendingApproval + registeredWithMWS;
			return text.GetHashCode();
		}

		public override string ToString()
		{
			return $"[SavedStorePurchaseData] SKU:{sku}, Price:{price}, CurrencyCode:{currencyCode}, Token:{purchaseInfoToken}, OrderID:{purchaseInfoOrderId}, Pending:{pendingApproval} Registered{registeredWithMWS}";
		}

		public static bool operator ==(SavedStorePurchaseData a, SavedStorePurchaseData b)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			if ((object)a == null || (object)b == null)
			{
				return false;
			}
			return a.sku == b.sku && a.price == b.price && a.currencyCode == b.currencyCode && a.purchaseInfoToken == b.purchaseInfoToken && a.purchaseInfoOrderId == b.purchaseInfoOrderId && a.pendingApproval == b.pendingApproval && a.registeredWithMWS == b.registeredWithMWS;
		}

		public static bool operator !=(SavedStorePurchaseData a, SavedStorePurchaseData b)
		{
			return !(a == b);
		}
	}
}
