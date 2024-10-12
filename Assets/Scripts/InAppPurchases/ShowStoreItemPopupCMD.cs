using Disney.ClubPenguin.CPModuleUtils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
	public class ShowStoreItemPopupCMD
	{
		private StoreItemPopupWindow storeItemPopupWindowPrefab;

		private RectTransform storeItemPopupParent;

		private StoreItemPopupWindow storeItemPopupWindowInstance;

		private SkuInfo purchaseSkuInfo;

		private Sprite itemSprite;

		private Button backButton;

		public ShowStoreItemPopupCMD(StoreItemPopupWindow storeItemPopupPrefab, RectTransform storeItemPopupParent, SkuInfo purSkuInfo, Sprite itemSprite, Button backButton)
		{
			storeItemPopupWindowPrefab = storeItemPopupPrefab;
			this.storeItemPopupParent = storeItemPopupParent;
			purchaseSkuInfo = purSkuInfo;
			this.itemSprite = itemSprite;
			this.backButton = backButton;
		}

		public void Execute()
		{
			storeItemPopupWindowInstance = (UnityEngine.Object.Instantiate(storeItemPopupWindowPrefab) as StoreItemPopupWindow);
			storeItemPopupWindowInstance.GetComponent<RectTransform>().SetParent(storeItemPopupParent, worldPositionStays: false);
			storeItemPopupWindowInstance.SetupPopupData(purchaseSkuInfo, itemSprite);
			StoreItemPopupWindow storeItemPopupWindow = storeItemPopupWindowInstance;
			storeItemPopupWindow.DismissStoreItemPopup = (StoreItemPopupWindow.DismissStoreItemPopupDelegate)Delegate.Combine(storeItemPopupWindow.DismissStoreItemPopup, new StoreItemPopupWindow.DismissStoreItemPopupDelegate(OnStoreItemPopupDismissed));
			HardwareBackButtonDispatcher.SetTargetClickHandler(storeItemPopupWindowInstance.closeButton);
		}

		private void OnStoreItemPopupDismissed(string itemName)
		{
			HardwareBackButtonDispatcher.SetTargetClickHandler(backButton);
			StoreItemPopupWindow storeItemPopupWindow = storeItemPopupWindowInstance;
			storeItemPopupWindow.DismissStoreItemPopup = (StoreItemPopupWindow.DismissStoreItemPopupDelegate)Delegate.Remove(storeItemPopupWindow.DismissStoreItemPopup, new StoreItemPopupWindow.DismissStoreItemPopupDelegate(OnStoreItemPopupDismissed));
			UnityEngine.Object.Destroy(storeItemPopupWindowInstance.gameObject);
		}
	}
}
