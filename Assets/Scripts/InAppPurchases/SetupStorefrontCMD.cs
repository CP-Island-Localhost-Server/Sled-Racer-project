using DevonLocalization.Core;
using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.ClubPenguin.SledRacer;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	public class SetupStorefrontCMD
	{
		public delegate void StoreFrontSetupCompletedDelegate();

		public StoreFrontSetupCompletedDelegate StoreFrontSetupCompleted;

		private IMWSClient mwsClient;

		private StoreFrontController storeFrontController;

		private StoreType storeType;

		private CommerceProcessor commerceProcessor;

		private IAPModel iapModel;

		private StoreItemPopupWindow storeItemPopupWindowPrefab;

		private RectTransform storeItemPopupParent;

		private LoadingOverlay loadingOverlay;

		private MessageDialogOverlay messageDialogOverlay;

		private SavedStorePurchasesCollection savedStorePurchaseCollection;

		private Button backButton;

		private float requestTimeoutSec;

		private bool setupStoreFrontTimedout;

		private bool setupStoreFrontComplete;

		public SetupStorefrontCMD(IMWSClient mwsClient, StoreFrontController storeFrontController, StoreType storeType, CommerceProcessor commerceProcessor, IAPModel iapModel, StoreItemPopupWindow storeItemPopupWindowPrefab, RectTransform storeItemPopupParent, LoadingOverlay loadingOverlay, MessageDialogOverlay messageDialogOverlay, SavedStorePurchasesCollection savedStorePurchaseCollection, Button backButton, float requestTimeoutSec)
		{
			this.mwsClient = mwsClient;
			this.storeFrontController = storeFrontController;
			this.storeType = storeType;
			this.commerceProcessor = commerceProcessor;
			this.iapModel = iapModel;
			this.storeItemPopupWindowPrefab = storeItemPopupWindowPrefab;
			this.storeItemPopupParent = storeItemPopupParent;
			this.loadingOverlay = loadingOverlay;
			this.messageDialogOverlay = messageDialogOverlay;
			this.savedStorePurchaseCollection = savedStorePurchaseCollection;
			this.backButton = backButton;
			this.requestTimeoutSec = requestTimeoutSec;
		}

		public void Execute()
		{
			setupStoreFrontComplete = false;
			iapModel.AllItemList.UnionWith(savedStorePurchaseCollection.GetAllPurchasedProdIds(includeMwsUnregistered: true));
			storeFrontController.IapModel = iapModel;
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				loadingOverlay.Hide();
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.network.connection.error")).Execute();
				return;
			}
			StoreFrontController obj = storeFrontController;
			obj.StoreItemButtonClicked = (StoreFrontController.StoreItemButtonDelegate)Delegate.Combine(obj.StoreItemButtonClicked, new StoreFrontController.StoreItemButtonDelegate(OnStoreItemButtonClicked));
			if (mwsClient.AuthToken == null)
			{
				iapModel.IapViewType = IAPViewType.GUEST;
				if (StoreFrontSetupCompleted != null)
				{
					StoreFrontSetupCompleted();
				}
			}
			else
			{
				loadingOverlay.Show(lockInputFocus: false);
				loadingOverlay.SetStatusText("Getting player card data.");
				mwsClient.GetPlayerCardData(OnAccountInformationReceived, forceReload: true);
				loadingOverlay.StartCoroutine(TimeoutCoRoutine());
			}
		}

		private IEnumerator TimeoutCoRoutine()
		{
			yield return new WaitForSeconds(requestTimeoutSec);
			if (!setupStoreFrontComplete)
			{
				setupStoreFrontTimedout = true;
				loadingOverlay.Hide();
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, Localizer.Instance.GetTokenTranslation("iap.commerce.error.billing.notsupported")).Execute();
			}
		}

		private void OnAccountInformationReceived(IGetPlayerCardDataResponse response)
		{
			if (setupStoreFrontTimedout)
			{
				return;
			}
			setupStoreFrontComplete = true;
			loadingOverlay.Hide();
			if (response.IsError)
			{
				string tokenTranslation = Localizer.Instance.GetTokenTranslation("iap.error.serviceunreachable");
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, tokenTranslation, response.StatusCode.ToString()).Execute();
				return;
			}
			if (response.PlayerCardData.Member)
			{
				iapModel.IapViewType = IAPViewType.MEMBER;
			}
			else
			{
				iapModel.IapViewType = IAPViewType.NONMEMBER;
			}
			if (StoreFrontSetupCompleted != null)
			{
				StoreFrontSetupCompleted();
			}
		}

		private void OnStoreItemButtonClicked(StoreItem storeItem)
		{
			UIManager uIManager = Service.Get<UIManager>();
			uIManager.ShowParentGate(delegate
			{
				new OnItemButtonClickedCMD(iapModel, commerceProcessor, storeItem, storeType, mwsClient, storeItemPopupWindowPrefab, storeItemPopupParent, storeFrontController, loadingOverlay, messageDialogOverlay, backButton).Execute();
			}, string.Empty, force: true);
		}
	}
}
