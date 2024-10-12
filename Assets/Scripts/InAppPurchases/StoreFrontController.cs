using DevonLocalization.Core;
using Disney.ClubPenguin.CPModuleUtils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InAppPurchases
{
    public class StoreFrontController : MonoBehaviour
    {
        public delegate void StoreItemButtonDelegate(StoreItem item);

        public const string CLAIMED_BUTTON_TOKEN = "iap.storeitem.storeitemview.itembutton.text";

        public StoreItemButtonDelegate StoreItemButtonClicked;

        public RectTransform itemHozGroupPanel;

        public Text currentItemDescription;

        public StoreItem itemControllerPrefab;

        public ScrollRectSnapTo scrollRectSnapTo;

        private IAPModel iapModel;

        private StoreItem currentStoreItem;

        private Dictionary<string, StoreItem> itemControllers;

        public IAPModel IapModel
        {
            get
            {
                return iapModel;
            }
            set
            {
                if (iapModel != null)
                {
                    IAPModel iAPModel = iapModel;
                    iAPModel.IapViewTypeChanged = (IAPModel.IapViewTypeChangedDelegate)Delegate.Remove(iAPModel.IapViewTypeChanged, new IAPModel.IapViewTypeChangedDelegate(OnIapViewTypeChanged));
                }
                iapModel = value;
                if (iapModel != null)
                {
                    IAPModel iAPModel2 = iapModel;
                    iAPModel2.IapViewTypeChanged = (IAPModel.IapViewTypeChangedDelegate)Delegate.Combine(iAPModel2.IapViewTypeChanged, new IAPModel.IapViewTypeChangedDelegate(OnIapViewTypeChanged));
                }
            }
        }

        private void Awake()
        {
            itemControllers = new Dictionary<string, StoreItem>();
            scrollRectSnapTo.ElementSelected += OnItemScrollSnap;
        }

        private void OnDestroy()
        {
            IapModel = null;
            itemHozGroupPanel = null;
            currentItemDescription = null;
            itemControllerPrefab = null;
            scrollRectSnapTo = null;
        }

        private void OnIapViewTypeChanged()
        {
            if (iapModel.IapViewType == IAPViewType.MEMBER)
            {
                string tokenTranslation = Localizer.Instance.GetTokenTranslation("iap.storeitem.storeitemview.itembutton.text");
                foreach (KeyValuePair<string, StoreItem> itemController in itemControllers)
                {
                    itemController.Value.ItemButtonText.text = tokenTranslation;
                }
            }
        }

        public void SetupStoreItemViews(List<SkuInfo> skuList, SavedStorePurchasesCollection savedStorePurchasesCollection)
        {
            if (iapModel == null)
            {
                throw new Exception("StoreFrontController must have iapModel assigned before setting up views.");
            }
            foreach (SkuInfo sku in skuList)
            {
                Sprite sprite = Resources.Load<Sprite>("ItemSprites/" + sku.sku);
                if (sprite == null)
                {
                    UnityEngine.Debug.LogWarning("Unable to load resource " + sku.sku + " in the ItemSprites folder. Check asset names.");
                }
                StoreItem storeItem = CreateStoreItem(sku, sprite);
                if (itemControllers.ContainsKey(storeItem.SKUInfo.sku))
                {
                    itemControllers[storeItem.SKUInfo.sku] = storeItem;
                }
                else
                {
                    itemControllers.Add(storeItem.SKUInfo.sku, storeItem);
                }
                if (currentStoreItem == null)
                {
                    currentStoreItem = storeItem;
                }
            }
            SetStoreItemViewsToClaimed(iapModel.AllItemList);
            SetStoreItemViewsToPending(savedStorePurchasesCollection.GetAllPendingProducts());
        }

        private StoreItem CreateStoreItem(SkuInfo skuInfo, Sprite itemSprite)
{
    StoreItem storeItem = UnityEngine.Object.Instantiate(itemControllerPrefab) as StoreItem;
    storeItem.GetComponent<RectTransform>().SetParent(itemHozGroupPanel, worldPositionStays: false);
    storeItem.SKUInfo = skuInfo;
    storeItem.SetPurchased(); // Automatically mark every item as purchased
    Debug.Log($"Item {skuInfo.title} set as purchased.");

    if (iapModel != null && iapModel.IapViewType == IAPViewType.MEMBER)
    {
        string tokenTranslation = Localizer.Instance.GetTokenTranslation("iap.storeitem.storeitemview.itembutton.text");
        storeItem.ItemButtonText.text = tokenTranslation;
    }

    StoreItem storeItem2 = storeItem;
    storeItem2.buttonClicked = (StoreItem.StoreItemButtonClicked)Delegate.Combine(storeItem2.buttonClicked, new StoreItem.StoreItemButtonClicked(OnPurchasebuttonPressed));
    StoreItem storeItem3 = storeItem;
    storeItem3.StoreItemClicked = (StoreItem.StoreItemClickedDelegate)Delegate.Combine(storeItem3.StoreItemClicked, new StoreItem.StoreItemClickedDelegate(OnStoreItemClicked));

    if (itemSprite != null)
    {
        storeItem.ItemImage.sprite = itemSprite;
    }
    return storeItem;
}


        private void SetStoreItemViewsToClaimed(ICollection<string> unlockedItems)
        {
            foreach (string unlockedItem in unlockedItems)
            {
                if (itemControllers.ContainsKey(unlockedItem))
                {
                    SetStoreItemViewToClaimed(unlockedItem);
                }
            }
        }

        private void OnItemScrollSnap(GameObject item)
        {
            StoreItem component = item.GetComponent<StoreItem>();
            currentItemDescription.text = component.SKUInfo.description;
            currentStoreItem.SetSelected(isSelected: false);
            currentStoreItem = component;
            currentStoreItem.SetSelected(isSelected: true);
        }

        private void OnPurchasebuttonPressed(StoreItem item)
{
    Debug.Log("Purchase button pressed, but overriding to force item ownership.");
    item.SetPurchased(); // Mark item as purchased directly, skipping the purchase process
    if (StoreItemButtonClicked != null)
    {
        StoreItemButtonClicked(item);
    }
    OnStoreItemClicked(item.GetComponent<RectTransform>().GetSiblingIndex());
}


        private void OnStoreItemClicked(int indexInParent)
        {
            scrollRectSnapTo.TweenToContentChildWithIndex(indexInParent);
        }

        public void Cleanup()
        {
            foreach (KeyValuePair<string, StoreItem> itemController in itemControllers)
            {
                StoreItem value = itemController.Value;
                value.buttonClicked = (StoreItem.StoreItemButtonClicked)Delegate.Remove(value.buttonClicked, new StoreItem.StoreItemButtonClicked(OnPurchasebuttonPressed));
                StoreItem value2 = itemController.Value;
                value2.StoreItemClicked = (StoreItem.StoreItemClickedDelegate)Delegate.Remove(value2.StoreItemClicked, new StoreItem.StoreItemClickedDelegate(OnStoreItemClicked));
            }
            itemControllers = null;
        }

        public Sprite GetSpriteByName(string key)
        {
            if (itemControllers.ContainsKey(key))
            {
                return itemControllers[key].ItemImage.sprite;
            }
            return null;
        }

        public void UpdateAllViewClaimedStates()
        {
            foreach (KeyValuePair<string, StoreItem> itemController in itemControllers)
            {
                if (iapModel.AllItemList.Contains(itemController.Key))
                {
                    SetStoreItemViewToClaimed(itemController.Key);
                }
            }
        }

        private void SetStoreItemViewsToPending(ICollection<SavedStorePurchaseData> pendingItems)
        {
            foreach (SavedStorePurchaseData pendingItem in pendingItems)
            {
                if (itemControllers.ContainsKey(pendingItem.sku))
                {
                    SetStoreItemViewToPending(pendingItem.sku);
                }
            }
        }

        public void SetStoreItemViewToClaimed(string itemSKU)
        {
            if (itemControllers.ContainsKey(itemSKU))
            {
                StoreItem storeItem = itemControllers[itemSKU];
                StoreItem storeItem2 = storeItem;
                storeItem2.buttonClicked = (StoreItem.StoreItemButtonClicked)Delegate.Remove(storeItem2.buttonClicked, new StoreItem.StoreItemButtonClicked(OnPurchasebuttonPressed));
                storeItem.SetPurchased();
            }
        }

        public void SetStoreItemViewToPending(string itemSKU)
        {
            if (itemControllers.ContainsKey(itemSKU))
            {
                StoreItem storeItem = itemControllers[itemSKU];
                StoreItem storeItem2 = storeItem;
                storeItem2.buttonClicked = (StoreItem.StoreItemButtonClicked)Delegate.Remove(storeItem2.buttonClicked, new StoreItem.StoreItemButtonClicked(OnPurchasebuttonPressed));
                storeItem.SetPending();
            }
        }
    }
}
