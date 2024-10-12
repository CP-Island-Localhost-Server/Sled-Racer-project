using DevonLocalization.Core;
using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InAppPurchases
{
	public class SetupPurchasableProductsCMD
	{
		private string appId;

		private string appVersion;

		private StoreType storeType;

		private CommerceErrorCodeTokens commErrorCodeTokens;

		private CommerceProcessor commerceProcessor;

		private StoreFrontController storeFrontController;

		private SavedStorePurchasesCollection savedStorePurchasesCollection;

		private List<SkuInfo> skuInfoList;

		private IAPModel iapModel;

		private IMWSClient mwsClient;

		private LoadingOverlay loadingOverlay;

		private MessageDialogOverlay messageDialogOverlay;

		private Action<bool> setupCompleteHandler;

		private float requestTimeoutSec;

		private bool getProductsTimedOut;

		private bool getProductsCompleted;

		private bool getSkusTimedOut;

		private bool getSkusCompleted;

		private bool getPlayerProductsTimedOut;

		private bool getPlayerProductsCompleted;

		public SetupPurchasableProductsCMD(string appId, string appVersion, StoreType storeType, CommerceErrorCodeTokens commErrorCodeTokens, CommerceProcessor commerceProcessor, StoreFrontController storeFrontController, SavedStorePurchasesCollection savedStorePurchasesCollection, IAPModel iapModel, IMWSClient mwsClient, LoadingOverlay loadingOverlay, MessageDialogOverlay messageDialogOverlay, Action<bool> setupCompleteHandler, float requestTimeoutSec)
		{
			this.appId = appId;
			this.appVersion = appVersion;
			this.storeType = storeType;
			this.commErrorCodeTokens = commErrorCodeTokens;
			this.commerceProcessor = commerceProcessor;
			this.storeFrontController = storeFrontController;
			this.savedStorePurchasesCollection = savedStorePurchasesCollection;
			this.iapModel = iapModel;
			this.mwsClient = mwsClient;
			this.loadingOverlay = loadingOverlay;
			this.messageDialogOverlay = messageDialogOverlay;
			this.setupCompleteHandler = setupCompleteHandler;
			this.requestTimeoutSec = requestTimeoutSec;
		}

		public void Execute()
		{
			loadingOverlay.Show(lockInputFocus: false);
			loadingOverlay.SetStatusText("Requesting products from MWS");
			mwsClient.GetIAPProductsForApp(appId, appVersion, storeType, OnIAPProductsResponse);
			loadingOverlay.StartCoroutine(TimeoutforGetProducts());
		}

		private IEnumerator TimeoutforGetProducts()
		{
			yield return new WaitForSeconds(requestTimeoutSec);
			if (!getProductsCompleted)
			{
				getProductsTimedOut = true;
				loadingOverlay.Hide();
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.commerce.error.billing.notsupported")).Execute();
			}
		}

		private void OnIAPProductsResponse(IGetIAPProductsResponse response)
		{
			if (getProductsTimedOut)
			{
				return;
			}
			getProductsCompleted = true;
			loadingOverlay.Hide();
			if (response.IsError)
			{
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.error.mwsgetpurchasableitemsfailed"), response.StatusCode.ToString()).Execute();
				return;
			}
			string[] array = ((List<ProductDetail>)response.Products.Products).ConvertAll((ProductDetail p) => p.Id).ToArray();
			if (array.Length == 0)
			{
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.error.mwsreturnednoitems")).Execute();
				return;
			}
			loadingOverlay.Show(lockInputFocus: false);
			loadingOverlay.SetStatusText("Retrieving SKU products from store.");
			if (!commerceProcessor.isBillingSupported() && commerceProcessor.getBillingUnsupportedReason() == 502)
			{
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.commerce.error.android.notloggedin")).Execute();
				loadingOverlay.Hide();
				return;
			}
			CommerceProcessor obj = commerceProcessor;
			obj.SkuInventoryResponse = (CommerceProcessor.SkuInventoryResponseSend)Delegate.Combine(obj.SkuInventoryResponse, new CommerceProcessor.SkuInventoryResponseSend(OnSKUInventoryResponse));
			commerceProcessor.GetSKUDetails(array);
			loadingOverlay.StartCoroutine(TimeoutforGetSkus());
		}

		private IEnumerator TimeoutforGetSkus()
		{
			yield return new WaitForSeconds(requestTimeoutSec);
			if (!getSkusCompleted)
			{
				getSkusTimedOut = true;
				loadingOverlay.Hide();
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.commerce.error.billing.notsupported")).Execute();
			}
		}

		private void OnSKUInventoryResponse(List<SkuInfo> skuInfoList, CommerceError cError)
		{
			if (!getSkusTimedOut)
			{
				getSkusCompleted = true;
				loadingOverlay.Hide();
				CommerceProcessor obj = commerceProcessor;
				obj.SkuInventoryResponse = (CommerceProcessor.SkuInventoryResponseSend)Delegate.Remove(obj.SkuInventoryResponse, new CommerceProcessor.SkuInventoryResponseSend(OnSKUInventoryResponse));
				if (cError.HasError())
				{
					new HandleCommerceErrorCMD(cError, commErrorCodeTokens, messageDialogOverlay, closeContextOnDialogDismissed: true).Execute();
					return;
				}
				if (skuInfoList.Count == 0)
				{
					new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.error.storereturnednoitems")).Execute();
					return;
				}
				if (iapModel.IapViewType == IAPViewType.GUEST)
				{
					storeFrontController.SetupStoreItemViews(skuInfoList, savedStorePurchasesCollection);
					return;
				}
				loadingOverlay.Show(lockInputFocus: false);
				loadingOverlay.SetStatusText("Loading players items from MWS.");
				this.skuInfoList = skuInfoList;
				GetAllPlayerProdsCMD getAllPlayerProdsCMD = new GetAllPlayerProdsCMD(iapModel, messageDialogOverlay, mwsClient, OnAllMemberClaimedItemsReceived);
				getAllPlayerProdsCMD.Execute();
				loadingOverlay.StartCoroutine(TimeoutforGetPlayerProducts());
			}
		}

		private IEnumerator TimeoutforGetPlayerProducts()
		{
			yield return new WaitForSeconds(requestTimeoutSec);
			if (!getPlayerProductsCompleted)
			{
				getPlayerProductsTimedOut = true;
				loadingOverlay.Hide();
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.commerce.error.billing.notsupported")).Execute();
			}
		}

		private void OnAllMemberClaimedItemsReceived(int numItemsClaimed)
		{
			if (!getPlayerProductsTimedOut)
			{
				getPlayerProductsCompleted = true;
				loadingOverlay.Hide();
				storeFrontController.SetupStoreItemViews(skuInfoList, savedStorePurchasesCollection);
				setupCompleteHandler(obj: true);
			}
		}
	}
}
