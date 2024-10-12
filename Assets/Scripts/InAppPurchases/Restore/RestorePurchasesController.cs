using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InAppPurchases.Restore
{
	[RequireComponent(typeof(CommerceProcessorGooglePlay), typeof(CommerceProcessorApple), typeof(CommerceProcessorInit))]
	public class RestorePurchasesController : MonoBehaviour
	{
		public delegate void RestorePurchasesCompletedDelegate(SavedStorePurchasesCollection savedStoreCollection);

		public delegate void RestorePurchasesFailedDelegate(string errorToken, int errorNumber);

		public RestorePurchasesCompletedDelegate RestoreCompleted;

		public RestorePurchasesFailedDelegate RestoreFailed;

		private CommerceProcessor commerceProcessor;

		private CommerceProcessorInit commerceProcessorInit;

		private CommerceErrorCodeTokens commerceErrorCodeTokens;

		public void RestorePurchases(string googlePlayToken)
		{
			if (commerceProcessor == null)
			{
				CreateCommerceProcessor(googlePlayToken);
			}
			float waitTime = (Application.platform != RuntimePlatform.Android) ? 0f : 1f;
			StartCoroutine(RestorePurchasesAfterCommerceSetup(waitTime));
		}

		private void OnDestory()
		{
			if (commerceProcessor != null)
			{
				UnityEngine.Object.Destroy(commerceProcessor.gameObject);
			}
		}

		private IEnumerator RestorePurchasesAfterCommerceSetup(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			AttemptRestore();
		}

		private void AttemptRestore()
		{
			if (!commerceProcessor.isBillingSupported() && commerceProcessor.getBillingUnsupportedReason() == 502)
			{
				if (RestoreFailed != null)
				{
					string errorToken = "iap.commerce.error.android.notloggedin.restore";
					RestoreFailed(errorToken, 502);
				}
			}
			else
			{
				CommerceProcessor obj = commerceProcessor;
				obj.PurchaseRestoreResponse = (CommerceProcessor.PurchaseRestoreResponseSend)Delegate.Combine(obj.PurchaseRestoreResponse, new CommerceProcessor.PurchaseRestoreResponseSend(OnRestorePurchasesResponse));
				commerceProcessor.RestorePurchases();
			}
		}

		private void CreateCommerceProcessor(string googlePlayToken)
		{
			commerceErrorCodeTokens = new CommerceErrorCodeTokens();
			if (commerceProcessorInit == null)
			{
				commerceProcessorInit = GetComponent<CommerceProcessorInit>();
			}
			commerceProcessor = commerceProcessorInit.GetCommerceProcessor();
			commerceProcessor.InitializeStore(googlePlayToken);
		}

		private void OnRestorePurchasesResponse(List<PurchaseInfo> purchaseInfoList, List<SkuInfo> skuInfoList, CommerceError commerceError = null)
		{
			CommerceProcessor obj = commerceProcessor;
			obj.PurchaseRestoreResponse = (CommerceProcessor.PurchaseRestoreResponseSend)Delegate.Remove(obj.PurchaseRestoreResponse, new CommerceProcessor.PurchaseRestoreResponseSend(OnRestorePurchasesResponse));
			if (commerceError.HasError())
			{
				if (RestoreFailed != null)
				{
					RestoreFailed(commerceErrorCodeTokens.GetTokenForCommerceErrorCode(commerceError.GetErrorNo()), commerceError.GetErrorNo());
				}
				return;
			}
			if (purchaseInfoList.Count == 0)
			{
				if (RestoreFailed != null)
				{
					string tokenForCommerceErrorCode = commerceErrorCodeTokens.GetTokenForCommerceErrorCode(401);
					RestoreFailed(tokenForCommerceErrorCode, 401);
				}
				return;
			}
			SavedStorePurchasesCollection savedStorePurchasesCollection = new SavedStorePurchasesCollection();
			using (List<PurchaseInfo>.Enumerator enumerator = purchaseInfoList.GetEnumerator())
			{
				PurchaseInfo purchaseInfo;
				while (enumerator.MoveNext())
				{
					purchaseInfo = enumerator.Current;
					SkuInfo skuInfo = skuInfoList.Find((SkuInfo x) => x.sku == purchaseInfo.sku);
					SavedStorePurchaseData savedStorePurchaseData = SavedStorePurchaseData.FromCommerceData(purchaseInfo, skuInfo, pendingApproval: false, registeredWithMWS: true);
					savedStorePurchasesCollection.UpdateSavedStorePurchase(savedStorePurchaseData);
				}
			}
			savedStorePurchasesCollection.SaveToDisk();
			if (RestoreCompleted != null)
			{
				RestoreCompleted(savedStorePurchasesCollection);
			}
		}
	}
}
