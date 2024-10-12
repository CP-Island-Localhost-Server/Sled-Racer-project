using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using InAppPurchases.BI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	public class OnItemButtonClickedCMD
	{
		private static string CONTEXT_IAPATTEMPT = "iap_attempt";

		private static string MESSAGE_ITEMID = "item_id";

		private static string ACTION_CLICK = string.Empty;

		private IAPModel iapModel;

		private CommerceProcessor commerceProcessor;

		private StoreItem storeItem;

		private StoreType storeType;

		private IMWSClient mwsClient;

		private StoreItemPopupWindow storeItemPopupWindowPrefab;

		private RectTransform storeItemPopupParent;

		private StoreFrontController storeFrontController;

		private LoadingOverlay loadingOverlay;

		private MessageDialogOverlay messageDialogOverlay;

		private Button backButton;

		public OnItemButtonClickedCMD(IAPModel iapModel, CommerceProcessor commerceProcessor, StoreItem storeItem, StoreType storeType, IMWSClient mwsClient, StoreItemPopupWindow storeItemPopupWindowPrefab, RectTransform storeItemPopupParent, StoreFrontController storeFrontController, LoadingOverlay loadingOverlay, MessageDialogOverlay messageDialogOverlay, Button backButton)
		{
			this.iapModel = iapModel;
			this.commerceProcessor = commerceProcessor;
			this.storeItem = storeItem;
			this.storeType = storeType;
			this.mwsClient = mwsClient;
			this.storeItemPopupWindowPrefab = storeItemPopupWindowPrefab;
			this.storeItemPopupParent = storeItemPopupParent;
			this.storeFrontController = storeFrontController;
			this.loadingOverlay = loadingOverlay;
			this.messageDialogOverlay = messageDialogOverlay;
			this.backButton = backButton;
		}

		public void Execute()
		{
			if (!iapModel.IsPurchasingItem)
			{
				iapModel.IsPurchasingItem = true;
				DeterminePurchaseAction();
			}
		}

		private void DeterminePurchaseAction()
		{
			switch (iapModel.IapViewType)
			{
			case IAPViewType.GUEST:
			case IAPViewType.NONMEMBER:
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add(MESSAGE_ITEMID, storeItem.SKUInfo.sku);
				new SendGameActionBICMD(CONTEXT_IAPATTEMPT, ACTION_CLICK, dictionary, iapModel.PlayerID).Execute();
				new NonmemberPurchaseItemCMD(commerceProcessor, storeItem.SKUInfo.sku, loadingOverlay).Execute();
				break;
			}
			case IAPViewType.MEMBER:
			{
				List<string> list = new List<string>();
				list.Add(storeItem.SKUInfo.sku);
				List<string> productIds = list;
				new MemberClaimItemCMD(storeType, productIds, mwsClient, iapModel, storeItemPopupWindowPrefab, storeItemPopupParent, storeItem.SKUInfo, storeFrontController.GetSpriteByName(storeItem.SKUInfo.sku), messageDialogOverlay, storeFrontController, loadingOverlay, backButton).Execute();
				break;
			}
			}
		}
	}
}
