using System;
using System.Collections.Generic;

namespace InAppPurchases
{
	public class CommerceErrorCodeTokens
	{
		private Dictionary<int, string> ErrorCodesToTokens;

		public CommerceErrorCodeTokens()
		{
			ErrorCodesToTokens = new Dictionary<int, string>
			{
				{
					100,
					"iap.commerce.error.billing.notsupported"
				},
				{
					200,
					"iap.commerce.error.inventory.general"
				},
				{
					201,
					"iap.commerce.error.inventory.noitemsreturned"
				},
				{
					300,
					"iap.commerce.error.purchase.general"
				},
				{
					301,
					"iap.commerce.error.purchase.alreadypurchased"
				},
				{
					302,
					"iap.commerce.error.purchase.noskuinfoattemptlookup"
				},
				{
					303,
					"iap.commerce.error.purchase.noskuinfodonotlookup"
				},
				{
					304,
					"iap.commerce.error.purchase.cancelledbyuser"
				},
				{
					305,
					"iap.commerce.error.purchase.deferred"
				},
				{
					400,
					"iap.commerce.error.purchaserestore.general"
				},
				{
					401,
					"iap.commerce.error.purchaserestore.nopurchases"
				},
				{
					402,
					"iap.commerce.error.purchaserestore.noinventorydetails"
				},
				{
					500,
					"iap.commerce.error.billing.notsupported"
				},
				{
					501,
					"LoginContext.Error.Connection"
				},
				{
					502,
					"iap.commerce.error.android.notloggedin"
				},
				{
					503,
					string.Empty
				}
			};
		}

		public string GetTokenForCommerceErrorCode(int commerceErrorCode)
		{
			if (!ErrorCodesToTokens.ContainsKey(commerceErrorCode))
			{
				throw new Exception("Invalid commerce error code specified: " + commerceErrorCode);
			}
			return ErrorCodesToTokens[commerceErrorCode];
		}
	}
}
