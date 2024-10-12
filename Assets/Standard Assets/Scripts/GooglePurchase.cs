using System.Collections.Generic;

public class GooglePurchase
{
	public enum GooglePurchaseState
	{
		Purchased,
		Canceled,
		Refunded
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

	public string productId
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

	public GooglePurchaseState purchaseState
	{
		get;
		private set;
	}

	public string purchaseToken
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

	public GooglePurchase(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("packageName"))
		{
			packageName = dict["packageName"].ToString();
		}
		if (dict.ContainsKey("orderId"))
		{
			orderId = dict["orderId"].ToString();
		}
		if (dict.ContainsKey("productId"))
		{
			productId = dict["productId"].ToString();
		}
		if (dict.ContainsKey("developerPayload"))
		{
			developerPayload = dict["developerPayload"].ToString();
		}
		if (dict.ContainsKey("type"))
		{
			type = (dict["type"] as string);
		}
		if (dict.ContainsKey("purchaseTime"))
		{
			purchaseTime = long.Parse(dict["purchaseTime"].ToString());
		}
		if (dict.ContainsKey("purchaseState"))
		{
			purchaseState = (GooglePurchaseState)int.Parse(dict["purchaseState"].ToString());
		}
		if (dict.ContainsKey("purchaseToken"))
		{
			purchaseToken = dict["purchaseToken"].ToString();
		}
		if (dict.ContainsKey("signature"))
		{
			signature = dict["signature"].ToString();
		}
		if (dict.ContainsKey("originalJson"))
		{
			originalJson = dict["originalJson"].ToString();
		}
	}

	public static List<GooglePurchase> fromList(List<object> items)
	{
		List<GooglePurchase> list = new List<GooglePurchase>();
		foreach (Dictionary<string, object> item in items)
		{
			list.Add(new GooglePurchase(item));
		}
		return list;
	}

	public override string ToString()
	{
		return $"<GooglePurchase> packageName: {packageName}, orderId: {orderId}, productId: {productId}, developerPayload: {developerPayload}, purchaseToken: {purchaseToken}, purchaseState: {purchaseState}, signature: {signature}, type: {type}, json: {originalJson}";
	}
}
