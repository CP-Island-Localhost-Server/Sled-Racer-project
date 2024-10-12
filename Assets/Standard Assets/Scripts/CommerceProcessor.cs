using System.Collections.Generic;
using UnityEngine;

public abstract class CommerceProcessor : MonoBehaviour
{
	public delegate void SkuInventoryResponseSend(List<SkuInfo> listSkus, CommerceError cError);

	public delegate void PlayerInventoryResponseSend(List<PurchaseInfo> playerInventoryList, CommerceError cError);

	public delegate void PurchaseResponseSend(PurchaseInfo Purchase, SkuInfo purSkuInfo, CommerceError cError);

	public delegate void PurchaseRestoreResponseSend(List<PurchaseInfo> listPurchases, List<SkuInfo> listSkus, CommerceError cError);

	private bool _billingSupported;

	private int _billingUnsupportedReason;

	public SkuInventoryResponseSend SkuInventoryResponse;

	public PlayerInventoryResponseSend PlayerInventoryResponse;

	public PurchaseResponseSend PurchaseResponse;

	public PurchaseRestoreResponseSend PurchaseRestoreResponse;

	public virtual void InitializeStore(string token = "")
	{
	}

	public virtual void GetSKUDetails(string[] sku_array = null)
	{
	}

	public virtual void RestorePurchases()
	{
	}

	public virtual void PurchaseProduct(string product)
	{
	}

	public virtual void SetTestMode(int testMode)
	{
	}

	public virtual void DisableStore()
	{
	}

	public virtual bool isBillingSupported()
	{
		return _billingSupported;
	}

	public virtual void setBillingSupported(bool supported)
	{
		_billingSupported = supported;
	}

	public virtual int getBillingUnsupportedReason()
	{
		return _billingUnsupportedReason;
	}

	public virtual void setBillingUnsupportedReason(int unsupported_reason)
	{
		_billingUnsupportedReason = unsupported_reason;
	}

	public virtual void sendPurchaseResponse(CommerceError cError)
	{
		sendPurchaseResponse(null, null, cError);
	}

	public virtual void sendPurchaseResponse(PurchaseInfo pi, SkuInfo si, CommerceError cError = null)
	{
		if (PurchaseResponse != null)
		{
			CommerceLog("sendPurchaseResponse: not null");
			if (cError == null)
			{
				cError = new CommerceError(0, string.Empty, string.Empty);
			}
			PurchaseResponse(pi, si, cError);
		}
		else
		{
			CommerceLog("sendPurchaseResponse: listner is null and not pointing to anything");
		}
	}

	public virtual void sendInventoryResponse(CommerceError cError)
	{
		sendInventoryResponse(null, cError);
	}

	public virtual void sendInventoryResponse(List<SkuInfo> sil, CommerceError cError = null)
	{
		if (SkuInventoryResponse != null)
		{
			CommerceLog("sendInventoryResponse: not null");
			if (cError == null)
			{
				cError = new CommerceError(0, string.Empty, string.Empty);
			}
			SkuInventoryResponse(sil, cError);
		}
		else
		{
			CommerceLog("sendInventoryResponse: is null");
		}
	}

	public virtual void sendPlayerInventoryResponse(CommerceError cError)
	{
		sendPlayerInventoryResponse(null, cError);
	}

	public virtual void sendPlayerInventoryResponse(List<PurchaseInfo> pil, CommerceError cError = null)
	{
		if (PlayerInventoryResponse != null)
		{
			CommerceLog("sendPlayerInventoryResponse: not null");
			if (cError == null)
			{
				cError = new CommerceError(0, string.Empty, string.Empty);
			}
			PlayerInventoryResponse(pil, cError);
		}
		else
		{
			CommerceLog("sendPlayerInventoryResponse: is null");
		}
	}

	public virtual void sendPurchaseRestoreResponse(CommerceError cError)
	{
		sendPurchaseRestoreResponse(null, null, cError);
	}

	public virtual void sendPurchaseRestoreResponse(List<PurchaseInfo> pil, List<SkuInfo> sil, CommerceError cError = null)
	{
		if (PurchaseRestoreResponse != null)
		{
			CommerceLog("sendPurchaseRestoreResponse: not null");
			if (cError == null)
			{
				cError = new CommerceError(0, string.Empty, string.Empty);
			}
			PurchaseRestoreResponse(pil, sil, cError);
		}
		else
		{
			CommerceLog("sendPurchaseRestoreResponse: is null");
		}
	}

	public void CommerceLog(string logInfo)
	{
		UnityEngine.Debug.Log(logInfo);
	}
}
