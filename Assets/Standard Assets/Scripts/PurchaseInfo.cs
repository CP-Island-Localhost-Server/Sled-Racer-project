using System.Collections.Generic;
using UnityEngine;

public class PurchaseInfo
{
	public enum PurchaseState
	{
		Purchased,
		Canceled,
		Refunded,
		Deferred,
		Failed
	}

	public string packageName
	{
		get;
		private set;
	}

	public string orderId
	{
		get;
		private set;
	}

	public string sku
	{
		get;
		private set;
	}

	public string developerPayload
	{
		get;
		private set;
	}

	public string type
	{
		get;
		private set;
	}

	public long purchaseTime
	{
		get;
		private set;
	}

	public PurchaseState purchaseState
	{
		get;
		private set;
	}

	public string token
	{
		get;
		private set;
	}

	public string signature
	{
		get;
		private set;
	}

	public string originalJson
	{
		get;
		private set;
	}

	public PurchaseInfo(GooglePurchase gp)
	{
		if (gp.orderId != null)
		{
			orderId = gp.orderId;
		}
		if (gp.productId != null)
		{
			sku = gp.productId;
		}
		if (gp.purchaseTime > 0)
		{
			purchaseTime = gp.purchaseTime;
		}
		if (gp.purchaseToken != null)
		{
			token = gp.purchaseToken;
		}
		purchaseState = GetPurchaseState(gp.purchaseState.ToString());
		UnityEngine.Debug.Log("PurchaseInfo: Google Purchase State is " + gp.purchaseState);
		UnityEngine.Debug.Log("PurchaseInfo: Purchase State is " + purchaseState);
		UnityEngine.Debug.Log("PurchaseInfo: IsPurchased " + IsPurchased());
	}

	public PurchaseInfo(string i_orderId, string i_sku, long i_purchaseTime, string i_token, string i_purchase_state)
	{
		orderId = i_orderId;
		sku = i_sku;
		purchaseTime = i_purchaseTime;
		token = i_token;
		purchaseState = GetPurchaseState(i_purchase_state);
	}

	public static List<PurchaseInfo> fromList(List<GooglePurchase> items)
	{
		List<PurchaseInfo> list = new List<PurchaseInfo>();
		foreach (GooglePurchase item in items)
		{
			list.Add(new PurchaseInfo(item));
		}
		return list;
	}

	public bool IsPurchased()
	{
		return purchaseState == PurchaseState.Purchased;
	}

	public override string ToString()
	{
		return $"<PurchaseInfo> orderId: {orderId}, productId: {sku}, purchaseTime: {purchaseTime}, purchaseToken: {token},";
	}

	private PurchaseState GetPurchaseState(string pt)
	{
		PurchaseState result = PurchaseState.Purchased;
		switch (pt)
		{
		case "Purchased":
			result = PurchaseState.Purchased;
			break;
		case "Canceled":
			result = PurchaseState.Canceled;
			break;
		case "Refunded":
			result = PurchaseState.Refunded;
			break;
		}
		return result;
	}
}
