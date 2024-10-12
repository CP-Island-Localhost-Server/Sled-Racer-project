using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;
using InAppPurchases.BI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	public class SetupCommerceCMD
	{
		public delegate void CommerceSetupCompletedDelegate(CommerceProcessor commerceProcessor);

		public CommerceSetupCompletedDelegate CommerceSetupCompleted;

		private CommerceProcessorInit commerceProcessorInit;

		private string googlePlayToken;

		private CommerceErrorCodeTokens commErrorCodeTokens;

		private StoreType storeType;

		private StoreItemPopupWindow storeItemPopupWindowPrefab;

		private IMWSClient mwsClient;

		private RectTransform storeItemPopupParent;

		private StoreFrontController storeFrontController;

		private SavedStorePurchasesCollection savedStorePurchasesCollection;

		private string appID;

		private CommerceProcessor commerceProcessor;

		private IAPModel iapModel;

		private LoadingOverlay loadingOverlay;

		private MessageDialogOverlay messageDialogOverlay;

		private Button backButton;

		public SetupCommerceCMD(CommerceProcessorInit commerceProcessorInit, IAPModel iapModel, string googlePlayToken, CommerceErrorCodeTokens commErrorCodeTokens, StoreType storeType, StoreItemPopupWindow storeItemPopupWindowPrefab, IMWSClient mwsClient, RectTransform storeItemPopupParent, StoreFrontController storeFrontController, SavedStorePurchasesCollection savedStorePurchasesCollection, string appID, LoadingOverlay loadingOverlay, MessageDialogOverlay messageDialogOverlay, Button backButton)
		{
			this.commerceProcessorInit = commerceProcessorInit;
			this.iapModel = iapModel;
			this.googlePlayToken = googlePlayToken;
			this.commErrorCodeTokens = commErrorCodeTokens;
			this.storeType = storeType;
			this.storeItemPopupWindowPrefab = storeItemPopupWindowPrefab;
			this.mwsClient = mwsClient;
			this.storeItemPopupParent = storeItemPopupParent;
			this.storeFrontController = storeFrontController;
			this.savedStorePurchasesCollection = savedStorePurchasesCollection;
			this.appID = appID;
			this.loadingOverlay = loadingOverlay;
			this.messageDialogOverlay = messageDialogOverlay;
			this.backButton = backButton;
		}

		public void Execute()
		{
			commerceProcessor = commerceProcessorInit.GetCommerceProcessor();
			if (commerceProcessor == null)
			{
				messageDialogOverlay.ShowStatusTextFromToken("iap.error.setupcommprocessorfailed");
				return;
			}
			CommerceProcessor obj = commerceProcessor;
			obj.PurchaseResponse = (CommerceProcessor.PurchaseResponseSend)Delegate.Combine(obj.PurchaseResponse, new CommerceProcessor.PurchaseResponseSend(HandleCommercePurchase));
			commerceProcessor.InitializeStore(googlePlayToken);
			if (CommerceSetupCompleted != null)
			{
				CommerceSetupCompleted(commerceProcessor);
			}
		}

		private void HandleCommercePurchase(PurchaseInfo purchaseInfo, SkuInfo purSkuInfo, CommerceError cError)
		{
			iapModel.IsPurchasingItem = false;
			if (cError.HasError())
			{
				loadingOverlay.Hide();
				new HandleCommerceErrorCMD(cError, commErrorCodeTokens, messageDialogOverlay, false, savedStorePurchasesCollection, purSkuInfo, purchaseInfo).Execute();
				if (cError.GetErrorNo() == 305)
				{
					storeFrontController.SetStoreItemViewToPending(purSkuInfo.sku);
				}
			}
			else
			{
				loadingOverlay.Show(lockInputFocus: false);
				loadingOverlay.SetStatusText("Registering purchase to MWS.");
				iapModel.AllItemList.Add(purSkuInfo.sku);
				new HandleCommercePurchaseCMD(purchaseInfo, purSkuInfo, storeType, appID, mwsClient, savedStorePurchasesCollection, OnPurchaseRegisteredWithMWS, loadingOverlay).Execute();
			}
		}

		private void OnPurchaseRegisteredWithMWS(IHTTPResponse response, SkuInfo purSkuInfo, bool skippedDueToEditor)
		{
			storeFrontController.SetStoreItemViewToClaimed(purSkuInfo.sku);
			loadingOverlay.Hide();
			if (response != null && !skippedDueToEditor && response.IsError)
			{
				string text = null;
				switch (storeType)
				{
				case StoreType.APPLE:
					text = "iap.error.registerapplepurchasefailed";
					break;
				case StoreType.GOOGLE_PLAY:
					text = "iap.error.registergooglepurchasefailed";
					break;
				default:
					text = string.Empty;
					break;
				}
				messageDialogOverlay.ShowStatusTextFromToken(text, response.StatusCode.ToString());
			}
			DisplayPurchase(purSkuInfo);
		}

		private void DisplayPurchase(SkuInfo purSkuInfo)
		{
			new SendPaymentActionBICMD(iapModel.PlayerID, purSkuInfo).Execute();
			new ShowStoreItemPopupCMD(storeItemPopupWindowPrefab, storeItemPopupParent, purSkuInfo, storeFrontController.GetSpriteByName(purSkuInfo.sku), backButton).Execute();
		}
	}
}
