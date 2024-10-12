using Prime31;
using System.Collections.Generic;

public class CommerceProcessorGooglePlay : CommerceProcessor
{
	private bool _listnersEnabled;

	private List<SkuInfo> _retrievedSkuInfo;

	private bool _skuLookupInProgress;

	private bool _purchaseInProgress;

	private bool _restoreInProgress;

	private PurchaseInfo _purchaseMissingSkuDetails;

	private void OnDestroy()
	{
		if (_listnersEnabled)
		{
			DisableGPListners();
		}
		GoogleIAB.unbindService();
	}

	private void OnDisable()
	{
		CommerceLog("OnDisable triggered");
		DisableGPListners();
	}

	private void EnableGPListners()
	{
		CommerceLog("initializeStore: Set up the appropriate listners");
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		_listnersEnabled = true;
	}

	private void DisableGPListners()
	{
		CommerceLog("DisableGPListners: Closing the listners attached to GoogleIABManager");
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		_listnersEnabled = false;
	}

	public override void InitializeStore(string token = "")
	{
		CommerceLog("initializeStore: with token " + token);
		GoogleIAB.init(token);
		EnableGPListners();
	}

	public override void DisableStore()
	{
		CommerceLog("disableStore: attempting to close store");
		DisableGPListners();
		GoogleIAB.unbindService();
	}

	private void billingSupportedEvent()
	{
		CommerceLog("billingSupportedEvent");
		setBillingSupported(supported: true);
	}

	private void billingNotSupportedEvent(string error)
	{
		CommerceLog("billingNotSupportedEvent: " + error);
		if (error == "Error checking for billing v3 support. (response: 3:Billing Unavailable)")
		{
			setBillingUnsupportedReason(502);
		}
		else
		{
			setBillingUnsupportedReason(500);
		}
	}

	public override void GetSKUDetails(string[] sku_array = null)
	{
		CommerceLog("getSKUDetails: Getting Product Details");
		if (!isBillingSupported())
		{
			CommerceError cError = new CommerceError(100, "Billing Not Supported", string.Empty);
			sendInventoryResponse(cError);
			sendPlayerInventoryResponse(cError);
			return;
		}
		if (!_skuLookupInProgress)
		{
			if (sku_array == null)
			{
				string[] skus = new string[0];
				GoogleIAB.queryInventory(skus);
			}
			else
			{
				GoogleIAB.queryInventory(sku_array);
			}
			_skuLookupInProgress = true;
		}
		else
		{
			CommerceLog("getSKUDetails: sku lookup already in progress, will not resubmit");
		}
		CommerceLog("getSKUDetails: Finishing getting product information");
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		CommerceLog($"queryInventorySucceededEvent. total purchases: {purchases.Count}, total skus: {skus.Count}");
		Utils.logObject(purchases);
		Utils.logObject(skus);
		_skuLookupInProgress = false;
		CommerceLog("Trying to get SkuInfo");
		List<SkuInfo> list = _retrievedSkuInfo = SkuInfo.fromList(skus, _retrievedSkuInfo);
		CommerceLog("queryInventorySucceededEvent: count of stored inventory events is " + list.Count);
		CommerceLog("loaded up inventorySku getting SkuInfo");
		CommerceLog(list.ToString());
		if (_purchaseMissingSkuDetails != null)
		{
			CommerceLog("queryInventorySucceededEvent: found missing purchase, attempting to repair " + _purchaseMissingSkuDetails.sku);
			string sku = _purchaseMissingSkuDetails.sku;
			SkuInfo skuFromList = SkuInfo.GetSkuFromList(list, sku);
			if (skuFromList != null)
			{
				CommerceLog("queryInventorySucceededEvent: Successfuly found missing sku details for purchase " + _purchaseMissingSkuDetails.sku);
				sendPurchaseResponse(_purchaseMissingSkuDetails, skuFromList);
			}
			else
			{
				CommerceLog("queryInventorySucceededEvent: failed to find missing sku details for purchase " + _purchaseMissingSkuDetails.sku);
				CommerceError cError = new CommerceError(303, "Purchase could not be mapped to any sku details, do not retry", string.Empty);
				sendPurchaseResponse(cError);
			}
			_purchaseMissingSkuDetails = null;
		}
		sendInventoryResponse(list);
		CommerceLog("Finished getting SkuInfo");
		CommerceLog("Trying to get Purchase Info");
		List<PurchaseInfo> list2 = PurchaseInfo.fromList(purchases);
		CommerceLog("loaded up inventorySku getting SkuInfo");
		CommerceLog(list2.ToString());
		sendPlayerInventoryResponse(list2);
		CommerceLog("Finished getting PurchaseInfo");
	}

	private void queryInventoryFailedEvent(string error)
	{
		CommerceLog("queryInventoryFailedEvent: " + error);
		_skuLookupInProgress = false;
		CommerceError commerceError = null;
		if (_purchaseMissingSkuDetails != null)
		{
			commerceError = new CommerceError(303, "Purchase could not be mapped to any sku details, lookup failed", string.Empty);
			sendPurchaseResponse(commerceError);
			_purchaseMissingSkuDetails = null;
		}
		commerceError = new CommerceError(200, "Inventory Query Failed", string.Empty);
		sendInventoryResponse(commerceError);
	}

	public override void RestorePurchases()
	{
		CommerceLog("RestorePurchases: Started");
		if (!isBillingSupported())
		{
			CommerceError cError = new CommerceError(100, "Billing Not Supported", string.Empty);
			sendPurchaseRestoreResponse(cError);
			return;
		}
		if (!_restoreInProgress)
		{
			_restoreInProgress = true;
			GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
			GoogleIABManager.queryInventorySucceededEvent += purchaseRestoreSucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += purchaseRestoreFailedEvent;
			CommerceLog("RestorePurchases: triggering query inventory");
			string[] skus = new string[0];
			GoogleIAB.queryInventory(skus);
		}
		else
		{
			CommerceLog("RestorePurchases: Restore already in progress, skipping");
		}
		CommerceLog("RestorePurchases: Finishing restore request ");
	}

	private void purchaseRestoreSucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		CommerceLog($"restorePurchasesSucceededEvent: total purchases: {purchases.Count}, total skus: {skus.Count}");
		Utils.logObject(purchases);
		Utils.logObject(skus);
		_restoreInProgress = false;
		GoogleIABManager.queryInventorySucceededEvent -= purchaseRestoreSucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= purchaseRestoreFailedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		CommerceLog("Trying to get SkuInfo");
		List<SkuInfo> list = _retrievedSkuInfo = SkuInfo.fromList(skus, _retrievedSkuInfo);
		CommerceLog("restorePurchasesSucceededEvent: count of stored inventory events is " + list.Count);
		CommerceLog("loaded up inventorySku getting SkuInfo");
		CommerceLog(list.ToString());
		List<PurchaseInfo> list2 = PurchaseInfo.fromList(purchases);
		CommerceLog("loaded up playerPurchases getting SkuInfo");
		CommerceLog(list2.ToString());
		sendPurchaseRestoreResponse(list2, list);
		CommerceLog("restorePurchasesSucceededEvent: Finished getting restored purchases");
	}

	private void purchaseRestoreFailedEvent(string error)
	{
		CommerceLog("purchaseRestoreFailedEvent: " + error);
		_restoreInProgress = false;
		GoogleIABManager.queryInventorySucceededEvent -= purchaseRestoreSucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= purchaseRestoreFailedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		CommerceError cError = new CommerceError(400, "Restore Query Failed", string.Empty);
		sendPurchaseRestoreResponse(cError);
	}

	public override void PurchaseProduct(string product)
	{
		CommerceLog("PurchaseProduct: Purchasing sku " + product);
		if (!isBillingSupported())
		{
			CommerceError cError = new CommerceError(100, "Billing Not Supported", string.Empty);
			sendPurchaseResponse(cError);
			return;
		}
		if (!_purchaseInProgress)
		{
			GoogleIAB.purchaseProduct(product);
			_purchaseInProgress = true;
		}
		else
		{
			CommerceLog("PurchaseProduct: Purhcase already in progress, skipping");
		}
		CommerceLog("PurchaseProduct: Finishing purchasing " + product);
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		CommerceLog("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		CommerceLog("purchaseSucceededEvent: GP object" + purchase);
		_purchaseInProgress = false;
		PurchaseInfo purchaseInfo = new PurchaseInfo(purchase);
		CommerceLog("purchaseSucceededEvent: purchase object" + purchaseInfo);
		SkuInfo skuFromList = SkuInfo.GetSkuFromList(_retrievedSkuInfo, purchaseInfo.sku);
		if (skuFromList == null)
		{
			CommerceLog("purchaseSucceededEvent: could not find stored sku info for sku " + purchaseInfo.sku);
			_purchaseMissingSkuDetails = purchaseInfo;
			CommerceLog("purchaseSucceededEvent: telling calling function to look up skus again for sku " + purchaseInfo.sku);
			GetSKUDetails(new string[1]
			{
				purchaseInfo.sku
			});
		}
		else
		{
			sendPurchaseResponse(purchaseInfo, skuFromList);
		}
	}

	private void purchaseFailedEvent(string error)
	{
		CommerceLog("purchaseFailedEvent: '" + error + "'");
		_purchaseInProgress = false;
		CommerceError commerceError = null;
		commerceError = ((!(error == "Unable to buy item (response: 7:Item Already Owned)")) ? new CommerceError(300, "Purchase Failed", string.Empty) : new CommerceError(301, "Item Already Owned", string.Empty));
		sendPurchaseResponse(commerceError);
		CommerceLog("purchaseFailedEvent: complete");
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		CommerceLog("consumePurchaseSucceededEvent: " + purchase);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		CommerceLog("consumePurchaseFailedEvent: " + error);
	}
}
