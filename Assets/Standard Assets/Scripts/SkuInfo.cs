using System.Collections.Generic;

public class SkuInfo
{
	public string title
	{
		get;
		private set;
	}

	public string price
	{
		get;
		private set;
	}

	public string type
	{
		get;
		private set;
	}

	public string description
	{
		get;
		private set;
	}

	public string sku
	{
		get;
		private set;
	}

	public string currencyCode
	{
		get;
		private set;
	}

	public string currencySymbol
	{
		get;
		private set;
	}

	public SkuInfo(GoogleSkuInfo gsi)
	{
		if (gsi.title != null)
		{
			title = gsi.title;
		}
		if (gsi.price != null)
		{
			price = gsi.price;
		}
		if (gsi.type != null)
		{
			type = gsi.type;
		}
		if (gsi.description != null)
		{
			description = gsi.description;
		}
		if (gsi.productId != null)
		{
			sku = gsi.productId;
		}
		currencyCode = "USD";
	}

	public SkuInfo(string i_title, string i_price, string i_type, string i_description, string i_sku, string i_currencyCode, string i_currencySymbol)
	{
		title = i_title;
		price = i_price;
		type = i_type;
		description = i_description;
		sku = i_sku;
		currencyCode = i_currencyCode;
		currencySymbol = i_currencySymbol;
	}

	public static List<SkuInfo> fromList(List<GoogleSkuInfo> items, List<SkuInfo> passedSkuInfos = null)
	{
		List<SkuInfo> list = new List<SkuInfo>();
		if (passedSkuInfos != null)
		{
			list = passedSkuInfos;
		}
		foreach (GoogleSkuInfo item in items)
		{
			if (GetSkuFromList(list, item.productId) == null)
			{
				list.Add(new SkuInfo(item));
			}
		}
		return list;
	}

	public static SkuInfo GetSkuFromList(List<SkuInfo> items, string sku_name)
	{
		if (items != null && items.Count > 0)
		{
			foreach (SkuInfo item in items)
			{
				if (item.sku == sku_name)
				{
					return item;
				}
			}
		}
		return null;
	}

	public override string ToString()
	{
		return $"<SkuInfo> title: {title}, price: {price}, type: {type}, description: {description}, productId: {sku}";
	}
}
