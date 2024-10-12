using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public class DMOAnalyticsSysInfo
	{
		private static string mIntBIServer = "http://blog.analytics.tapulous.com:8088";

		private static string mBIServer = "https://api.disney.com/dismo/bi/v1";

		public static readonly string mVersion = "3.1.6";

		public static readonly string appStart = "app_start";

		public static readonly string appForeground = "app_foreground";

		public static readonly string appBackground = "app_background";

		public static readonly string appEnd = "app_end";

		public static readonly string gameAction = "game_action";

		public static readonly string moneyAction = "money";

		public static readonly string iOSMoneyAction = "payment_action";

		public static string CellophaneBIKey = "3962215E-C235-4FA8-BD8D-78BB9DA7D3B4";

		public static string CellophaneBISecret = "AC364A88C2D12A352DC81DC5FBBC9FEEE8BCCB3D9CA155FB";

		public static string CellophaneIntBIKey = "334E04DB-F88C-4FAA-8D82-0680EFC01364";

		public static string CellophaneIntBISecret = "2846EAA366F423DCA0E175B07D82770E918B8D27CD5B904E";

		public static bool isIntServerEnvironment;

		private static string mBundleIdentifier = "com.disney.HelloUnity";

		private static string mSessionHash;

		private static string mToken;

		private static string mAppVersion = "1.0";

		private static int kCCBlockSizeAES128 = 16;

		private static int kCCKeySizeAES128 = 16;

		public static string getAppVersion()
		{
			return mAppVersion;
		}

		public static void setAppVersion(string appVersion)
		{
			mAppVersion = appVersion;
		}

		public static string getAnalyticsURL()
		{
			if (isIntServerEnvironment)
			{
				return mIntBIServer;
			}
			return mBIServer;
		}

		public static string getCellophaneKey()
		{
			if (isIntServerEnvironment)
			{
				return CellophaneIntBIKey;
			}
			return CellophaneBIKey;
		}

		public static string getCellophaneSecret()
		{
			if (isIntServerEnvironment)
			{
				return CellophaneIntBISecret;
			}
			return CellophaneBISecret;
		}

		public static string getAnalyticsKey()
		{
			return DMOAnalytics.SharedAnalytics.AnalyticsKey;
		}

		public static string getAnalyticsSecret()
		{
			return DMOAnalytics.SharedAnalytics.AnalyticsSecret;
		}

		public static void setBundelIdentifer(string bundleID)
		{
			mBundleIdentifier = bundleID;
		}

		public static string getBundleId()
		{
			return mBundleIdentifier;
		}

		public static string getModel()
		{
			return SystemInfo.deviceModel;
		}

		public static string getSystemVersion()
		{
			return SystemInfo.operatingSystem;
		}

		public static string getIsNewUser()
		{
			string result = "0";
			string @string = PlayerPrefs.GetString("DMOIsNewUser");
			if (string.IsNullOrEmpty(@string))
			{
				result = "1";
				PlayerPrefs.SetString("DMOIsNewUser", "0");
			}
			return result;
		}

		private static string encryptString(string rawData)
		{
			if (rawData == null)
			{
				return null;
			}
			string result = string.Empty;
			byte[] bytes = Encoding.UTF8.GetBytes(rawData);
			int num = bytes.Length;
			if (num % kCCBlockSizeAES128 > 0)
			{
				num = (num + kCCBlockSizeAES128) / kCCBlockSizeAES128 * kCCBlockSizeAES128;
			}
			byte[] array = new byte[num];
			Array.Copy(bytes, array, bytes.Length);
			int num2 = 20;
			byte[] array2 = new byte[20]
			{
				45,
				55,
				162,
				205,
				40,
				214,
				99,
				105,
				236,
				91,
				177,
				233,
				174,
				28,
				10,
				94,
				12,
				192,
				243,
				93
			};
			byte[] array3 = new byte[kCCKeySizeAES128];
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				if (i != 1 && i != 4 && i != 11 && i != 19)
				{
					array3[num3++] = array2[i];
				}
			}
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Mode = CipherMode.ECB;
			rijndaelManaged.Padding = PaddingMode.None;
			rijndaelManaged.Key = array3;
			ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
			byte[] array4 = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			if (array4 != null && array4.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < array4.Length; j++)
				{
					stringBuilder.AppendFormat("{0:x2}", array4[j] & 0xFF);
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		public static string _getUniqueIdentifier()
		{
			string text = null;
			if (string.IsNullOrEmpty(text))
			{
				string @string = PlayerPrefs.GetString("DMOCurrentUniqueID");
				if (string.IsNullOrEmpty(@string))
				{
					text = Guid.NewGuid().ToString().ToUpper();
					PlayerPrefs.SetString("DMOCurrentUniqueID", text);
				}
				else
				{
					text = @string;
				}
			}
			return text;
		}

		public static string getMToken()
		{
			if (string.IsNullOrEmpty(mToken))
			{
				string rawData = _getUniqueIdentifier();
				mToken = encryptString(rawData);
			}
			return mToken;
		}

		public static string getSessionId()
		{
			if (mSessionHash == null || mSessionHash.Length == 0)
			{
				mSessionHash = Guid.NewGuid().ToString().ToUpper();
			}
			return mSessionHash;
		}

		public static string getUTCTime()
		{
			return DateTime.UtcNow.ToString();
		}

		public static string getOnlineFlag()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
			{
				return "1";
			}
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
			{
				return "2";
			}
			return "0";
		}

		public static string getTimeStamp()
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime value = Convert.ToDateTime("1/1/1970 0:00:00 AM");
			TimeSpan timeSpan = utcNow.Subtract(value);
			return ((long)(((timeSpan.Days * 24 + timeSpan.Hours) * 60 + timeSpan.Minutes) * 60 + timeSpan.Seconds)).ToString();
		}

		public static Dictionary<string, object> GetSysInfo()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("DMOLibVersion", mVersion);
			dictionary.Add("api_key", getAnalyticsKey());
			dictionary.Add("app_version", getAppVersion());
			dictionary.Add("bundle_id", getBundleId());
			dictionary.Add("m_token", getMToken());
			dictionary.Add("model", getModel());
			dictionary.Add("onlineflag", getOnlineFlag());
			dictionary.Add("os_version", getSystemVersion());
			dictionary.Add("session_hash", getSessionId());
			dictionary.Add("timestamp", getTimeStamp());
			return dictionary;
		}

		public static Dictionary<string, object> GetCoreEventInfo()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("api_key", getAnalyticsKey());
			dictionary.Add("bundle_id", getBundleId());
			dictionary.Add("m_token", getMToken());
			dictionary.Add("onlineflag", getOnlineFlag());
			dictionary.Add("os_version", getSystemVersion());
			dictionary.Add("session_hash", getSessionId());
			dictionary.Add("timestamp", getTimeStamp());
			return dictionary;
		}

		public static Dictionary<string, object> GetTestDataDict()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("UTCTime", "2013-08-21+15:47:26+-25200+(PDT)");
			dictionary.Add("api_key", "2FD4ECB8-64B1-4610-BD3F-FF934AFA0FE1");
			dictionary.Add("bundle_id", "com.disney.abtest");
			dictionary.Add("ios_advertising_id", "2104d1f15a83ed00a95c123bea12d0c2b07a711e2b73c2f71ed50d8736583c0bbb7fb168f034b8456272afb06acb7e53");
			dictionary.Add("ios_vendor_id", "77945b5e1e28f990244a5c0837a9d339ebf22f785e369a0e7b648e10326b2d38123b79642c997e270f6b9126b8f58855");
			dictionary.Add("is_new_user", "0");
			dictionary.Add("m_token", "5d6f6d21fe429b7d80bf88872e897000f9c4bc73dd294336c20b1f92c7e3e703");
			dictionary.Add("method", "app_foreground");
			dictionary.Add("onlineflag", "2");
			dictionary.Add("session_hash", "791890AE-C8E8-4F15-95FC-314A929B8EDD");
			dictionary.Add("tag", "clicked_link");
			dictionary.Add("timestamp", "1377125247");
			return dictionary;
		}
	}
}
