using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;
using System.Collections.Generic;

namespace InAppPurchases
{
	public class ReRegisterFailedPurchasesCMD
	{
		public delegate void RegistrationCompletedDelegate(List<SavedStorePurchaseData> registeredPurchases, List<SavedStorePurchaseData> failedPurchases, List<IHTTPResponse> failedPurchaseResponses);

		public delegate void RegistrationStatusUpdatedDelegate(string statusMessage);

		public RegistrationCompletedDelegate RegistrationCompleted;

		public RegistrationStatusUpdatedDelegate RegistrationStatusUpdated;

		private IMWSClient mwsClient;

		private StoreType storeType;

		private string appId;

		private SavedStorePurchaseData currentPurchaseData;

		private List<SavedStorePurchaseData>.Enumerator unregisteredPurchasesEnumerator;

		private List<SavedStorePurchaseData> registeredPurchases;

		private List<SavedStorePurchaseData> failedPurchases;

		private List<IHTTPResponse> failedPurchaseResponses;

		private List<SavedStorePurchaseData> unregisteredPurchases;

		private SavedStorePurchasesCollection savedStorePurchaseCollection;

		private LoadingOverlay loadingOverlay;

		public ReRegisterFailedPurchasesCMD(IMWSClient mwsClient, StoreType storeType, string appId, LoadingOverlay loadingOverlay)
		{
			this.mwsClient = mwsClient;
			this.storeType = storeType;
			this.appId = appId;
			this.loadingOverlay = loadingOverlay;
			registeredPurchases = new List<SavedStorePurchaseData>();
			failedPurchases = new List<SavedStorePurchaseData>();
			failedPurchaseResponses = new List<IHTTPResponse>();
		}

		public void Execute()
		{
			savedStorePurchaseCollection = new SavedStorePurchasesCollection();
			savedStorePurchaseCollection.LoadFromDisk();
			unregisteredPurchases = savedStorePurchaseCollection.GetAllMWSUnregisteredProducts();
			unregisteredPurchasesEnumerator = unregisteredPurchases.GetEnumerator();
			RegisterNextPurchase();
		}

		private void RegisterNextPurchase()
		{
			if (unregisteredPurchasesEnumerator.Current == null)
			{
				if (RegistrationCompleted != null)
				{
					RegistrationCompleted(registeredPurchases, failedPurchases, failedPurchaseResponses);
				}
				return;
			}
			currentPurchaseData = unregisteredPurchasesEnumerator.Current;
			PurchaseInfo purchaseInfo = currentPurchaseData.GetPurchaseInfo();
			SkuInfo skuInfo = currentPurchaseData.GetSkuInfo();
			if (RegistrationStatusUpdated != null)
			{
				RegistrationStatusUpdated("Attepting to Re-register purchase for sku: " + skuInfo.sku);
			}
			new HandleCommercePurchaseCMD(purchaseInfo, skuInfo, storeType, appId, mwsClient, savedStorePurchaseCollection, OnPurchaseRegistered, loadingOverlay).Execute();
		}

		private void OnPurchaseRegistered(IHTTPResponse response, SkuInfo skuInfo, bool skippedDueToEditor)
		{
			if (response.IsError)
			{
				if (response.StatusCode >= 500)
				{
					savedStorePurchaseCollection.RemoveSavedStorePurchase(currentPurchaseData);
					savedStorePurchaseCollection.SaveToDisk();
				}
				failedPurchases.Add(currentPurchaseData);
				failedPurchaseResponses.Add(response);
			}
			else
			{
				currentPurchaseData.registeredWithMWS = true;
				savedStorePurchaseCollection.UpdateSavedStorePurchase(currentPurchaseData);
				savedStorePurchaseCollection.SaveToDisk();
				registeredPurchases.Add(currentPurchaseData);
			}
			unregisteredPurchasesEnumerator.MoveNext();
			RegisterNextPurchase();
		}
	}
}
