using System;
using System.IO;
using UnityEngine;

public class GooglePlayDownloader
{
	private const string Environment_MEDIA_MOUNTED = "mounted";

	private static AndroidJavaClass detectAndroidJNI;

	private static AndroidJavaClass Environment;

	private static string obb_package;

	private static int obb_version;

	static GooglePlayDownloader()
	{
		if (RunningOnAndroid())
		{
			Environment = new AndroidJavaClass("android.os.Environment");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderService"))
			{
				androidJavaClass.SetStatic("BASE64_PUBLIC_KEY", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqK1nPVlanbMpEX28Fxt6PM2uwq4NxUrzTK95rvqlsH178ZoV9+lzeyiDn81CUOUrSCcvQ+mEqHf7yg4T/aMv94FQYpjCt5L0xTXmAzdYUOz4Kqgww42Vx+9BvJmGTkuQogUx0Imqgn34JbYfsUocrq1+iWxMnn89DmKSqIYluOR1Q+SKA0HvuOyFAvMB3OyFQ/AgV3HzxL/aUgRbi6NiD1BDxZAVnS71pF0+k/EBTPLGeGrv2KcrFMwRz5qvSPWButkW+doR2IQnjAU8vArT6hwDwLV23ggtEuu7tzXSxSoXh77fwFyrilNPWl5q1hknNEwZCbTEuunJ4CMX3plz0QIDAQAB");
				androidJavaClass.SetStatic("SALT", new byte[20]
				{
					1,
					43,
					244,
					255,
					54,
					98,
					156,
					244,
					43,
					2,
					248,
					252,
					9,
					5,
					150,
					148,
					223,
					45,
					255,
					84
				});
			}
		}
	}

	public static bool RunningOnAndroid()
	{
		if (detectAndroidJNI == null)
		{
			detectAndroidJNI = new AndroidJavaClass("android.os.Build");
		}
		return detectAndroidJNI.GetRawClass() != IntPtr.Zero;
	}

	public static string GetExpansionFilePath()
	{
		populateOBBData();
		if (Environment.CallStatic<string>("getExternalStorageState", new object[0]) != "mounted")
		{
			return null;
		}
		using (AndroidJavaObject androidJavaObject = Environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]))
		{
			string arg = androidJavaObject.Call<string>("getPath", new object[0]);
			return string.Format("{0}/{1}/{2}", arg, "Android/obb", obb_package);
			IL_006e:
			string result;
			return result;
		}
	}

	public static string GetMainOBBPath(string expansionFilePath)
	{
		populateOBBData();
		if (expansionFilePath == null)
		{
			return null;
		}
		string text = $"{expansionFilePath}/main.{obb_version}.{obb_package}.obb";
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	public static string GetPatchOBBPath(string expansionFilePath)
	{
		populateOBBData();
		if (expansionFilePath == null)
		{
			return null;
		}
		string text = $"{expansionFilePath}/patch.{obb_version}.{obb_package}.obb";
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	public static void FetchOBB()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", @static, new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderActivity"));
			int num = 65536;
			androidJavaObject.Call<AndroidJavaObject>("addFlags", new object[1]
			{
				num
			});
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
			{
				"unityplayer.Activity",
				@static.Call<AndroidJavaObject>("getClass", new object[0]).Call<string>("getName", new object[0])
			});
			@static.Call("startActivity", androidJavaObject);
			if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
			{
				UnityEngine.Debug.LogError("Exception occurred while attempting to start DownloaderActivity - is the AndroidManifest.xml incorrect?");
				AndroidJNI.ExceptionDescribe();
				AndroidJNI.ExceptionClear();
			}
		}
	}

	private static void populateOBBData()
	{
		if (obb_version == 0)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				obb_package = @static.Call<string>("getPackageName", new object[0]);
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]).Call<AndroidJavaObject>("getPackageInfo", new object[2]
				{
					obb_package,
					0
				});
				obb_version = androidJavaObject.Get<int>("versionCode");
			}
		}
	}
}
