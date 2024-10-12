using Disney.ClubPenguin.Service.MWS;
using Disney.ClubPenguin.Service.MWS.Domain;
using System;
using System.Collections.Generic;

namespace InAppPurchases
{
	public class GetAllPlayerProdsCMD
	{
		private IAPModel iapModel;

		private MessageDialogOverlay messageDialogOverlay;

		private IMWSClient mwsClient;

		private Action<int> responseHandler;

		public GetAllPlayerProdsCMD(IAPModel iapModel, MessageDialogOverlay messageDialogOverlay, IMWSClient mwsClient, Action<int> responseHandler)
		{
			this.iapModel = iapModel;
			this.messageDialogOverlay = messageDialogOverlay;
			this.mwsClient = mwsClient;
			this.responseHandler = responseHandler;
		}

		public void Execute()
		{
			mwsClient.GetIAPPurchasesForPlayer(null, OnAllMemberClaimedItemsReceived);
		}

		private void OnAllMemberClaimedItemsReceived(IGetIAPPurchasesResponse response)
		{
			if (response.IsError)
			{
				messageDialogOverlay.ShowStatusTextFromToken("iap.error.getallclaimeditemsfailed", response.StatusCode.ToString());
				responseHandler(0);
				return;
			}
			if (response.Products.Count > 0)
			{
				List<string> list = new List<string>();
				foreach (ProductPurchase product in response.Products)
				{
					if (product != null)
					{
						list.Add(product.ProductId);
					}
				}
				iapModel.AllItemList.UnionWith(list);
			}
			responseHandler(response.Products.Count);
		}
	}
}
