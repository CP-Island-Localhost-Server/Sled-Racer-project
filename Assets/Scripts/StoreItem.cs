using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class StoreItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler
{
    public delegate void StoreItemClickedDelegate(int indexInParent);

    public delegate void StoreItemButtonClicked(StoreItem item);

    public StoreItemClickedDelegate StoreItemClicked;

    public StoreItemButtonClicked buttonClicked;

    public Text ItemNameText;

    public Text ItemButtonText;

    public Image ItemImage;

    public Image LockIconImage;

    public Image CheckMarkImage;

    public Image HighlightedImage;

    public Image UnselectedBGImage;

    public Button PurchaseButton;

    public Text AlreadyOwnedText;

    public Text PendingText;

    public AudioClip ButtonClickSound;

    private SkuInfo skuInfo;

    public SkuInfo SKUInfo
    {
        get
        {
            return skuInfo;
        }
        set
        {
            skuInfo = value;
            if (skuInfo != null)
            {
                string title = skuInfo.title;
                string[] array = title.Split('(');
                ItemNameText.text = array[0];
                ItemButtonText.text = skuInfo.price.Replace("₽", "руб.");
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (StoreItemClicked != null)
        {
            StoreItemClicked(GetComponent<RectTransform>().GetSiblingIndex());
        }
    }

    public void SetPurchased()
{
    // Forcefully mark item as purchased
    CheckMarkImage.gameObject.SetActive(true);
    LockIconImage.gameObject.SetActive(false);
    AlreadyOwnedText.gameObject.SetActive(true);
    PurchaseButton.gameObject.SetActive(false);
    PendingText.gameObject.SetActive(false);
    Debug.Log("Item successfully marked as purchased.");
}


    public void SetPending()
    {
        CheckMarkImage.gameObject.SetActive(value: false);
        LockIconImage.gameObject.SetActive(value: true);
        AlreadyOwnedText.gameObject.SetActive(value: false);
        PurchaseButton.gameObject.SetActive(value: false);
        PendingText.gameObject.SetActive(value: true);
    }

    public void SetAvailable()
    {
        CheckMarkImage.gameObject.SetActive(value: false);
        LockIconImage.gameObject.SetActive(value: false);
        AlreadyOwnedText.gameObject.SetActive(value: false);
        PurchaseButton.gameObject.SetActive(value: true);
        PendingText.gameObject.SetActive(value: false);
    }

    public void SetUnavailable()
    {
        CheckMarkImage.gameObject.SetActive(value: false);
        LockIconImage.gameObject.SetActive(value: true);
        AlreadyOwnedText.gameObject.SetActive(value: false);
        PurchaseButton.gameObject.SetActive(value: false);
        PendingText.gameObject.SetActive(value: false);
    }

    public void OnButtonHandler()
    {
        GetComponent<AudioSource>().PlayOneShot(ButtonClickSound);
        SetPurchased(); // Mark the item as purchased immediately when the button is clicked
        if (buttonClicked != null)
        {
            buttonClicked(this);
        }
    }

    public void SetSelected(bool isSelected)
    {
        HighlightedImage.gameObject.SetActive(isSelected);
        UnselectedBGImage.gameObject.SetActive(!isSelected);
    }
}
