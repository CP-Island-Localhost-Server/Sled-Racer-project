using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.DMOAnalytics.Framework;
using Disney.HTTP.Client;
using System;
using System.Collections;
using UnityEngine;

namespace InAppPurchases
{
	public class HandleCommercePurchaseCMD
	{
		private PurchaseInfo purchaseInfo;

		private SkuInfo skuInfo;

		private StoreType storeType;

		private string appId;

		private IMWSClient mwsClient;

		private SavedStorePurchasesCollection savedStorePurchasesCollection;

		private Action<IHTTPResponse, SkuInfo, bool> registrationCompletedHandler;

		private SavedStorePurchaseData ssp;

		private LoadingOverlay loadingOverlay;

		private float requestTimeoutSec = 20f;

		private bool registerItemWithMWSTimedOut;

		private bool registerItemWithMWSCompleted;

		public HandleCommercePurchaseCMD(PurchaseInfo purchaseInfo, SkuInfo skuInfo, StoreType storeType, string appId, IMWSClient mwsClient, SavedStorePurchasesCollection savedStorePurchasesCollection, Action<IHTTPResponse, SkuInfo, bool> registrationCompletedHandler, LoadingOverlay loadingOverlay)
		{
			this.purchaseInfo = purchaseInfo;
			this.skuInfo = skuInfo;
			this.storeType = storeType;
			this.appId = appId;
			this.mwsClient = mwsClient;
			this.savedStorePurchasesCollection = savedStorePurchasesCollection;
			this.registrationCompletedHandler = registrationCompletedHandler;
			this.loadingOverlay = loadingOverlay;
		}

		public void Execute()
		{
			ssp = savedStorePurchasesCollection.GetPurchasedProductById(skuInfo.sku);
			if (ssp == null)
			{
				ssp = SavedStorePurchaseData.FromCommerceData(purchaseInfo, skuInfo, pendingApproval: false, registeredWithMWS: false);
				savedStorePurchasesCollection.UpdateSavedStorePurchase(ssp);
				savedStorePurchasesCollection.SaveToDisk();
			}
			else
			{
				ssp.pendingApproval = false;
			}
			if (string.IsNullOrEmpty(mwsClient.AuthToken))
			{
				registrationCompletedHandler(null, skuInfo, arg3: false);
			}
			else if (Application.isEditor)
			{
				UnityEngine.Debug.LogWarning("Unsupported platform, not sending purchase to MWSClient.");
				registrationCompletedHandler(new HTTPErrorResponse(404, "Not available in unity editor"), skuInfo, arg3: true);
			}
			else if (storeType == StoreType.APPLE)
			{
				ApplePurchaseData applePurchaseData = new ApplePurchaseData();
				applePurchaseData.ReceiptId = ssp.purchaseInfoToken;
				applePurchaseData.Language = Application.systemLanguage.ToString();
				applePurchaseData.Price = ssp.price;
				applePurchaseData.Currency = ssp.currencyCode;
				mwsClient.RegisterApplePurchase(appId, DMOAnalyticsSysInfo._getUniqueIdentifier(), applePurchaseData, OnRegisterPurchaseComplete);
				loadingOverlay.StartCoroutine(TimeoutforGetSkus());
			}
			else if (storeType == StoreType.GOOGLE_PLAY)
			{
				GooglePlayPurchaseData googlePlayPurchaseData = new GooglePlayPurchaseData();
				googlePlayPurchaseData.SubscriptionId = ssp.sku;
				googlePlayPurchaseData.Token = ssp.purchaseInfoToken;
				googlePlayPurchaseData.Language = Application.systemLanguage.ToString();
				googlePlayPurchaseData.Price = ssp.price;
				googlePlayPurchaseData.Currency = ssp.currencyCode;
				mwsClient.RegisterGooglePlayPurchase(appId, DMOAnalyticsSysInfo._getUniqueIdentifier(), googlePlayPurchaseData, OnRegisterPurchaseComplete);
				loadingOverlay.StartCoroutine(TimeoutforGetSkus());
			}
		}

		private IEnumerator TimeoutforGetSkus()
		{
			yield return new WaitForSeconds(requestTimeoutSec);
			if (!registerItemWithMWSCompleted)
			{
				registerItemWithMWSTimedOut = true;
				registrationCompletedHandler(new HTTPErrorResponse(404, string.Empty), skuInfo, arg3: false);
			}
		}

		private void OnRegisterPurchaseComplete(IHTTPResponse response)
		{
			if (!registerItemWithMWSTimedOut)
			{
				registerItemWithMWSCompleted = true;
				if (!response.IsError)
				{
					ssp.registeredWithMWS = true;
					savedStorePurchasesCollection.UpdateSavedStorePurchase(ssp);
					savedStorePurchasesCollection.SaveToDisk();
				}
				registrationCompletedHandler(response, skuInfo, arg3: false);
			}
		}
	}
}
