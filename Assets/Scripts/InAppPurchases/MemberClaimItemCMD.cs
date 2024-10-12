using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.DMOAnalytics.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	public class MemberClaimItemCMD
	{
		private StoreType storeType;

		private List<string> productIds;

		private IMWSClient mwsClient;

		public IAPModel iapModel;

		private StoreItemPopupWindow storeItemWindowPrefab;

		private RectTransform storeItemPopupParent;

		private SkuInfo skuInfo;

		private Sprite storeItemImage;

		private MessageDialogOverlay messageDialogOverlay;

		private StoreFrontController storeFrontController;

		private LoadingOverlay loadingOverlay;

		private Button backButton;

		public MemberClaimItemCMD(StoreType storeType, List<string> productIds, IMWSClient mwsClient, IAPModel iapModel, StoreItemPopupWindow storeItemWindowPrefab, RectTransform storeItemPopupParent, SkuInfo skuInfo, Sprite storeItemImage, MessageDialogOverlay messageDialogOverlay, StoreFrontController storeFrontController, LoadingOverlay loadingOverlay, Button backButton)
		{
			this.storeType = storeType;
			this.productIds = productIds;
			this.mwsClient = mwsClient;
			this.iapModel = iapModel;
			this.storeItemWindowPrefab = storeItemWindowPrefab;
			this.storeItemPopupParent = storeItemPopupParent;
			this.skuInfo = skuInfo;
			this.storeItemImage = storeItemImage;
			this.messageDialogOverlay = messageDialogOverlay;
			this.storeFrontController = storeFrontController;
			this.loadingOverlay = loadingOverlay;
			this.backButton = backButton;
		}

		public void Execute()
		{
			loadingOverlay.Show(lockInputFocus: false);
			loadingOverlay.SetStatusText("Claiming member item with MWS.");
			mwsClient.ClaimProductsForMember(DMOAnalyticsSysInfo._getUniqueIdentifier(), storeType, productIds, OnItemsClaimedForMember);
		}

		private void OnItemsClaimedForMember(IClaimProductsForMemberResponse response)
		{
			iapModel.IsPurchasingItem = false;
			if (response.StatusCode == 200)
			{
				iapModel.AllItemList.Add(skuInfo.sku);
				storeFrontController.SetStoreItemViewToClaimed(skuInfo.sku);
				loadingOverlay.Hide();
				new ShowStoreItemPopupCMD(storeItemWindowPrefab, storeItemPopupParent, skuInfo, storeItemImage, backButton).Execute();
			}
			else
			{
				loadingOverlay.Hide();
				messageDialogOverlay.ShowStatusTextFromToken("iap.error.claimnewitemfailed");
			}
		}
	}
}
