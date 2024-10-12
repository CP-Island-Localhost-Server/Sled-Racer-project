using Disney.ClubPenguin.CPModuleUtils;
using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.HTTP.Client;
using System;
using System.Collections.Generic;

namespace InAppPurchases
{
	public class ShowReRegistrationAttempCMD
	{
		private IMWSClient mwsClient;

		private StoreType storeType;

		private string appId;

		private MessageDialogOverlay messageDialogOverlay;

		private LoadingOverlay loadingOverlay;

		private ReRegisterFailedPurchasesCMD reRegisterCMD;

		public ShowReRegistrationAttempCMD(IMWSClient mwsClient, StoreType storeType, string appId, MessageDialogOverlay messageDialogOverlay, LoadingOverlay loadingOverlay)
		{
			this.mwsClient = mwsClient;
			this.storeType = storeType;
			this.appId = appId;
			this.messageDialogOverlay = messageDialogOverlay;
			this.loadingOverlay = loadingOverlay;
		}

		public void Execute()
		{
			loadingOverlay.Show(lockInputFocus: false);
			loadingOverlay.SetStatusText("Attempting to register previous purchases with MWS due to past failure.");
			reRegisterCMD = new ReRegisterFailedPurchasesCMD(mwsClient, storeType, appId, loadingOverlay);
			ReRegisterFailedPurchasesCMD reRegisterFailedPurchasesCMD = reRegisterCMD;
			reRegisterFailedPurchasesCMD.RegistrationCompleted = (ReRegisterFailedPurchasesCMD.RegistrationCompletedDelegate)Delegate.Combine(reRegisterFailedPurchasesCMD.RegistrationCompleted, new ReRegisterFailedPurchasesCMD.RegistrationCompletedDelegate(OnRegistrationCompleted));
			ReRegisterFailedPurchasesCMD reRegisterFailedPurchasesCMD2 = reRegisterCMD;
			reRegisterFailedPurchasesCMD2.RegistrationStatusUpdated = (ReRegisterFailedPurchasesCMD.RegistrationStatusUpdatedDelegate)Delegate.Combine(reRegisterFailedPurchasesCMD2.RegistrationStatusUpdated, new ReRegisterFailedPurchasesCMD.RegistrationStatusUpdatedDelegate(OnRegistrationStatusUpdate));
			reRegisterCMD.Execute();
		}

		private void OnRegistrationCompleted(List<SavedStorePurchaseData> registeredPurchases, List<SavedStorePurchaseData> failedPurchases, List<IHTTPResponse> failedPurchaseResponses)
		{
			ReRegisterFailedPurchasesCMD reRegisterFailedPurchasesCMD = reRegisterCMD;
			reRegisterFailedPurchasesCMD.RegistrationCompleted = (ReRegisterFailedPurchasesCMD.RegistrationCompletedDelegate)Delegate.Remove(reRegisterFailedPurchasesCMD.RegistrationCompleted, new ReRegisterFailedPurchasesCMD.RegistrationCompletedDelegate(OnRegistrationCompleted));
			ReRegisterFailedPurchasesCMD reRegisterFailedPurchasesCMD2 = reRegisterCMD;
			reRegisterFailedPurchasesCMD2.RegistrationStatusUpdated = (ReRegisterFailedPurchasesCMD.RegistrationStatusUpdatedDelegate)Delegate.Remove(reRegisterFailedPurchasesCMD2.RegistrationStatusUpdated, new ReRegisterFailedPurchasesCMD.RegistrationStatusUpdatedDelegate(OnRegistrationStatusUpdate));
			loadingOverlay.Hide();
			if (failedPurchases.Count > 0)
			{
				messageDialogOverlay.ShowStatusTextFromString("There were problems re-registering some purchased products.");
			}
		}

		private void OnRegistrationStatusUpdate(string updateMessage)
		{
			loadingOverlay.SetStatusText(updateMessage);
		}
	}
}
