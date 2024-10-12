using System.Collections.Generic;

namespace InAppPurchases
{
	public class IAPModel
	{
		public delegate void IapViewTypeChangedDelegate();

		public IapViewTypeChangedDelegate IapViewTypeChanged;

		private IAPViewType iapViewType;

		public bool IsPurchasingItem;

		public HashSet<string> AllItemList;

		public long PlayerID;

		public IAPViewType IapViewType
		{
			get
			{
				return iapViewType;
			}
			set
			{
				iapViewType = value;
				if (IapViewTypeChanged != null)
				{
					IapViewTypeChanged();
				}
			}
		}

		public IAPModel()
		{
			AllItemList = new HashSet<string>();
		}

		public override string ToString()
		{
			string text = $"[IAPModel: IapViewType={IapViewType}]";
			foreach (string allItem in AllItemList)
			{
				text = text + allItem + "\n";
			}
			return text;
		}
	}
}
