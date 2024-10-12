using Disney.ClubPenguin.CPModuleUtils;
using System;

namespace InAppPurchases
{
	public class NonmemberPurchaseItemCMD
	{
		private CommerceProcessor commerceProcessor;

		private string productName;

		private LoadingOverlay loadingOverlay;

		public NonmemberPurchaseItemCMD(CommerceProcessor commerceProcessor, string productName, LoadingOverlay loadingOverlay)
		{
			this.commerceProcessor = commerceProcessor;
			this.productName = productName;
			this.loadingOverlay = loadingOverlay;
		}

		public void Execute()
		{
			loadingOverlay.Show(lockInputFocus: false);
			loadingOverlay.SetStatusText("Purchasing product...");
			CommerceProcessor obj = commerceProcessor;
			obj.PurchaseResponse = (CommerceProcessor.PurchaseResponseSend)Delegate.Combine(obj.PurchaseResponse, new CommerceProcessor.PurchaseResponseSend(OnPurchaseReceived));
			commerceProcessor.PurchaseProduct(productName);
		}

		private void OnPurchaseReceived(PurchaseInfo purchaseInfo, SkuInfo sku, CommerceError error)
		{
			CommerceProcessor obj = commerceProcessor;
			obj.PurchaseResponse = (CommerceProcessor.PurchaseResponseSend)Delegate.Remove(obj.PurchaseResponse, new CommerceProcessor.PurchaseResponseSend(OnPurchaseReceived));
		}
	}
}
