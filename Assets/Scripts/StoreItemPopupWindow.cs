using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class StoreItemPopupWindow : MonoBehaviour
{
    public delegate void DismissStoreItemPopupDelegate(string itemName);

    public DismissStoreItemPopupDelegate DismissStoreItemPopup;

    public Text itemName;

    public Image itemImage;

    public Button closeButton;

    public AudioClip purchaseSound;

    public AudioClip closeButtonSound;

    private SkuInfo skuInfo;

    private void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(purchaseSound);
    }

    public void SetupPopupData(SkuInfo skuInfo, Sprite itemSprite)
    {
        this.skuInfo = skuInfo;
        itemImage.sprite = itemSprite;
        string title = skuInfo.title;
        string[] array = title.Split('(');
        itemName.text = array[0];

        // Directly mark the item as purchased if applicable (you may need to adjust this logic based on your existing setup)
        if (skuInfo != null) // Check if SKUInfo indicates the item should be treated as purchased
        {
            Debug.Log("Item should be marked as purchased."); // Add your purchase logic here if needed
        }

        closeButton.onClick.AddListener(delegate
        {
            GetComponent<AudioSource>().PlayOneShot(closeButtonSound);
            StartCoroutine(WaitForSound());
        });
    }

    private IEnumerator WaitForSound()
    {
        yield return null;
        if (DismissStoreItemPopup != null)
        {
            DismissStoreItemPopup(skuInfo.sku);
        }
    }
}
