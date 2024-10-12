using System.Collections.Generic;

public class CommerceProcessorMock : CommerceProcessor
{
	private int _testMode = 1;

	public override void InitializeStore(string token = "")
	{
		CommerceLog("initializeStore: mock store");
		if (_testMode > 0)
		{
			setBillingSupported(supported: true);
		}
	}

	public override void GetSKUDetails(string[] sku_array = null)
	{
		CommerceLog("getSKUDetails: Mock Getting Product Details");
		CommerceError commerceError = null;
		List<SkuInfo> list = new List<SkuInfo>();
		switch (_testMode)
		{
		case 0:
			commerceError = new CommerceError(100, "Mock Object general failure", string.Empty);
			sendInventoryResponse(commerceError);
			break;
		case 1:
			CommerceLog("Starting to generate fake sku list");
			foreach (string text2 in sku_array)
			{
				SkuInfo item2 = new SkuInfo(text2 + " title", "$1.99", "inapp", "Product 1 Desc", text2, "USD", "$");
				CommerceLog("Adding sku to list");
				list.Add(item2);
			}
			CommerceLog("Calling Inventory Response");
			sendInventoryResponse(list);
			break;
		case 2:
			commerceError = new CommerceError(200, "Mock Object general failure", string.Empty);
			sendInventoryResponse(commerceError);
			break;
		case 3:
			CommerceLog("Starting to generate fake sku list");
			foreach (string text in sku_array)
			{
				SkuInfo item = new SkuInfo(text + " title", "$1.99", "inapp", "Product 1 Desc", text, "USD", "$");
				CommerceLog("Adding sku to list");
				list.Add(item);
			}
			CommerceLog("Calling Inventory Response");
			sendInventoryResponse(list);
			break;
		default:
			CommerceLog("Mock Object: getSkuDetails does not cover this case");
			break;
		}
	}

	public override void PurchaseProduct(string product)
	{
		CommerceLog("Mock PurchaseProduct: Purchasing sku " + product);
		CommerceError commerceError = null;
		switch (_testMode)
		{
		case 0:
			commerceError = new CommerceError(100, "Mock Object general failure", string.Empty);
			sendPurchaseResponse(commerceError);
			break;
		case 1:
		{
			PurchaseInfo pi = new PurchaseInfo("1001", product, 1413836611000L, "product1_token", "Purchased");
			SkuInfo si = new SkuInfo("Product 1", "$1.99", "Non-Consumable", "Product 1 Desc", product, "USD", "$");
			sendPurchaseResponse(pi, si);
			break;
		}
		case 2:
			commerceError = new CommerceError(300, "Mock Object general failure", string.Empty);
			sendPurchaseResponse(commerceError);
			break;
		case 3:
			commerceError = new CommerceError(305, "Apple Ask To Buy", string.Empty);
			sendPurchaseResponse(commerceError);
			break;
		default:
			CommerceLog("Mock Object: PurchaseProduct does not cover this case");
			break;
		}
	}

	public override void RestorePurchases()
	{
		CommerceLog("Mock RestorePurchases: Started");
		CommerceError commerceError = null;
		SkuInfo item = new SkuInfo("Product 1", "$1.99", "Non-Consumable", "Product 1 Desc", "com.cp.product1", "USD", "$");
		List<SkuInfo> list = new List<SkuInfo>();
		list.Add(item);
		PurchaseInfo item2 = new PurchaseInfo("1001", "com.cp.product1", 1413836611000L, "product1_token", "Purchased");
		List<PurchaseInfo> list2 = new List<PurchaseInfo>();
		list2.Add(item2);
		switch (_testMode)
		{
		case 0:
			commerceError = new CommerceError(100, "Mock Object general failure", string.Empty);
			sendPurchaseRestoreResponse(commerceError);
			break;
		case 1:
			sendPurchaseRestoreResponse(list2, list);
			break;
		case 2:
			commerceError = new CommerceError(400, "Mock Object general failure", string.Empty);
			sendPurchaseRestoreResponse(commerceError);
			break;
		case 3:
			sendPurchaseRestoreResponse(list2, list);
			break;
		default:
			CommerceLog("Mock Object: getSkuDetails does not cover this case");
			break;
		}
	}

	public override void SetTestMode(int testMode)
	{
		_testMode = testMode;
	}
}
