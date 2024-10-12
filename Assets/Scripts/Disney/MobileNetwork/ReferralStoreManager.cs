using System;
using UnityEngine;

namespace Disney.MobileNetwork
{
	public class ReferralStoreManager : MonoBehaviour, IInitializable, IPlugin
	{
		private LoggerHelper m_logger = new LoggerHelper();

		public static ReferralStoreManager Instance;

		public LoggerHelper Logger => m_logger;

		public static event Action<string> StoreShowEvent;

		public static event Action<string> StoreHideEvent;

		public void SetLogger(LoggerHelper.LoggerDelegate loggerMessageHandler)
		{
			m_logger.LogMessageHandler += loggerMessageHandler;
		}

		public void Init()
		{
		}

		private void Awake()
		{
			Instance = this;
			base.name = "ReferralStoreManager";
		}

		public virtual void Show()
		{
			Logger.LogDebug(this, "ShowReferralStore called in editor");
		}

		public void DidShowMoreDisney(string message)
		{
			Logger.LogDebug(this, "Referral Store UI shown with message = " + message);
			if (ReferralStoreManager.StoreShowEvent != null)
			{
				ReferralStoreManager.StoreShowEvent(message);
			}
		}

		public void DidHideMoreDisney(string message)
		{
			Logger.LogDebug(this, "Referral Store UI closed with message = " + message);
			if (ReferralStoreManager.StoreHideEvent != null)
			{
				ReferralStoreManager.StoreHideEvent(message);
			}
		}
	}
}
