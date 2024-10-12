using Disney.MobileNetwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace InAppPurchases
{
	[Serializable]
	public class SavedStorePurchasesCollection
	{
		private const string FILE_NAME = "/storeItems.dat";

		private const char ELEMENT_DELIMITER = ',';

		private const char LIST_DELIMITER = '|';

		public List<SavedStorePurchaseData> PurchasedItemsData;

		public string EncryptedData;

		private string defaultFilePath;

		public SavedStorePurchasesCollection()
		{
			defaultFilePath = Application.persistentDataPath + "/storeItems.dat";
			PurchasedItemsData = new List<SavedStorePurchaseData>();
		}

		public void UpdateSavedStorePurchase(SavedStorePurchaseData savedStorePurchaseData)
		{
			RemoveSavedStorePurchase(savedStorePurchaseData);
			PurchasedItemsData.Add(savedStorePurchaseData);
		}

		public void RemoveSavedStorePurchase(SavedStorePurchaseData savedStorePurchaseData)
		{
			if (PurchasedItemsData.Contains(savedStorePurchaseData))
			{
				PurchasedItemsData.Remove(savedStorePurchaseData);
			}
		}

		public bool ExistOnDisk()
		{
			return File.Exists(defaultFilePath);
		}

		public ICollection<string> GetAllPurchasedProdIds(bool includeMwsUnregistered = false)
		{
			List<string> list = new List<string>();
			foreach (SavedStorePurchaseData purchasedItemsDatum in PurchasedItemsData)
			{
				if (!purchasedItemsDatum.pendingApproval && (includeMwsUnregistered || purchasedItemsDatum.registeredWithMWS))
				{
					list.Add(purchasedItemsDatum.sku);
				}
			}
			return list;
		}

		public SavedStorePurchaseData GetPurchasedProductById(string productId)
		{
			foreach (SavedStorePurchaseData purchasedItemsDatum in PurchasedItemsData)
			{
				if (purchasedItemsDatum.sku == productId)
				{
					return purchasedItemsDatum;
				}
			}
			return null;
		}

		public List<SavedStorePurchaseData> GetAllPendingProducts()
		{
			List<SavedStorePurchaseData> list = new List<SavedStorePurchaseData>();
			foreach (SavedStorePurchaseData purchasedItemsDatum in PurchasedItemsData)
			{
				if (purchasedItemsDatum.pendingApproval)
				{
					list.Add(purchasedItemsDatum);
				}
			}
			return list;
		}

		public List<SavedStorePurchaseData> GetAllMWSUnregisteredProducts()
		{
			List<SavedStorePurchaseData> list = new List<SavedStorePurchaseData>();
			foreach (SavedStorePurchaseData purchasedItemsDatum in PurchasedItemsData)
			{
				if (!purchasedItemsDatum.registeredWithMWS && !purchasedItemsDatum.pendingApproval)
				{
					list.Add(purchasedItemsDatum);
				}
			}
			return list;
		}

		public void LoadFromDisk()
		{
			if (ExistOnDisk())
			{
				SetupSerialization();
				FileStream fileStream = File.Open(defaultFilePath, FileMode.Open);
				if (fileStream.Length < 1)
				{
					UnityEngine.Debug.Log(defaultFilePath + " was empty.");
					fileStream.Close();
					RestoreSerialization();
					return;
				}
				SavedStorePurchasesCollection savedStorePurchasesCollection = null;
				try
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					savedStorePurchasesCollection = (SavedStorePurchasesCollection)binaryFormatter.Deserialize(fileStream);
				}
				catch (Exception)
				{
					UnityEngine.Debug.Log("File was corrupted. Removing file.");
					fileStream.Close();
					RestoreSerialization();
					ClearFile();
					return;
					IL_0089:;
				}
				PurchasedItemsData = savedStorePurchasesCollection.PurchasedItemsData;
				string text = EncryptionHelper.DecryptFile(defaultFilePath, savedStorePurchasesCollection.EncryptedData);
				if (string.IsNullOrEmpty(text) && PurchasedItemsData.Count > 0)
				{
					UnityEngine.Debug.LogError("Order and token ids were empty although purchases exist.");
					fileStream.Close();
					RestoreSerialization();
					ClearFile();
					return;
				}
				string[] array = text.Split('|');
				if (array.Length != 2)
				{
					UnityEngine.Debug.LogError("Invalid structure for merged order and token ids.");
					fileStream.Close();
					RestoreSerialization();
					ClearFile();
					return;
				}
				string[] array2 = array[0].Split(',');
				string[] array3 = array[1].Split(',');
				if (array2.Length != array3.Length || PurchasedItemsData.Count != array2.Length)
				{
					UnityEngine.Debug.LogError("token and order id count mismatch.");
					fileStream.Close();
					RestoreSerialization();
					ClearFile();
					return;
				}
				for (int i = 0; i < PurchasedItemsData.Count; i++)
				{
					PurchasedItemsData[i].purchaseInfoOrderId = array2[i];
					PurchasedItemsData[i].purchaseInfoToken = array3[i];
				}
				fileStream.Close();
				RestoreSerialization();
			}
			else
			{
				UnityEngine.Debug.LogWarning(defaultFilePath + " does not exist.");
			}
		}

		public void SaveToDisk()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			for (int i = 0; i < PurchasedItemsData.Count; i++)
			{
				if (i > 0)
				{
					text += ',';
					text2 += ',';
				}
				text += PurchasedItemsData[i].purchaseInfoOrderId;
				text2 += PurchasedItemsData[i].purchaseInfoToken;
			}
			string fileContent = text + '|' + text2;
			SetupSerialization();
			EncryptedData = EncryptionHelper.EncryptFile(defaultFilePath, fileContent);
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				using (FileStream fileStream = File.Create(defaultFilePath))
				{
					binaryFormatter.Serialize(fileStream, this);
					fileStream.Flush();
					fileStream.Close();
				}
			}
			finally
			{
				RestoreSerialization();
			}
		}

		private void SetupSerialization()
		{
		}

		private void RestoreSerialization()
		{
		}

		public void ClearFile()
		{
			try
			{
				List<SavedStorePurchaseData> graph = new List<SavedStorePurchaseData>();
				SetupSerialization();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Create(defaultFilePath);
				binaryFormatter.Serialize(fileStream, graph);
				fileStream.Close();
				RestoreSerialization();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("Unable to clear file " + defaultFilePath + ". Received " + ex.Message);
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			foreach (SavedStorePurchaseData purchasedItemsDatum in PurchasedItemsData)
			{
				text = text + purchasedItemsDatum.ToString() + "\n";
			}
			return text;
		}
	}
}
