using DevonLocalization.Core;

namespace InAppPurchases
{
	public class HandleCommerceErrorCMD
	{
		private CommerceError commerceError;

		private CommerceErrorCodeTokens commErrorCodeTokens;

		private MessageDialogOverlay messageDialogOverlay;

		private bool closeContextOnDialogDismissed;

		private SkuInfo skuInfo;

		private PurchaseInfo purchaseInfo;

		private SavedStorePurchasesCollection savedStorePurchasesCollection;

		public HandleCommerceErrorCMD(CommerceError commerceError, CommerceErrorCodeTokens commErrorCodeTokens, MessageDialogOverlay messageDialogOverlay, bool closeContextOnDialogDismissed = false, SavedStorePurchasesCollection savedStorePurchasesCollection = null, SkuInfo skuInfo = null, PurchaseInfo purchaseInfo = null)
		{
			this.commerceError = commerceError;
			this.commErrorCodeTokens = commErrorCodeTokens;
			this.messageDialogOverlay = messageDialogOverlay;
			this.closeContextOnDialogDismissed = closeContextOnDialogDismissed;
			this.savedStorePurchasesCollection = savedStorePurchasesCollection;
			this.skuInfo = skuInfo;
			this.purchaseInfo = purchaseInfo;
		}

		public void Execute()
		{
			string tokenForCommerceErrorCode = commErrorCodeTokens.GetTokenForCommerceErrorCode(commerceError.GetErrorNo());
			string tokenTranslation = Localizer.Instance.GetTokenTranslation(tokenForCommerceErrorCode);
			if (commerceError.GetErrorNo() == 305)
			{
				SavedStorePurchaseData savedStorePurchaseData = SavedStorePurchaseData.FromCommerceData(purchaseInfo, skuInfo, pendingApproval: true, registeredWithMWS: false);
				savedStorePurchasesCollection.UpdateSavedStorePurchase(savedStorePurchaseData);
				savedStorePurchasesCollection.SaveToDisk();
			}
			if (closeContextOnDialogDismissed)
			{
				new ShowDialogAndCloseContextCMD(messageDialogOverlay, tokenTranslation, commerceError.GetErrorNo().ToString()).Execute();
			}
			else
			{
				messageDialogOverlay.ShowStatusTextFromString(tokenTranslation, commerceError.GetErrorNo().ToString());
			}
		}
	}
}
