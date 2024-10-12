using UnityEngine;

namespace InAppPurchases
{
	public class ForceCloseAllIapContextsCMD
	{
		public void Execute()
		{
			IAPContext[] array = Object.FindObjectsOfType(typeof(IAPContext)) as IAPContext[];
			IAPContext[] array2 = array;
			foreach (IAPContext iAPContext in array2)
			{
				iAPContext.Close();
			}
		}
	}
}
