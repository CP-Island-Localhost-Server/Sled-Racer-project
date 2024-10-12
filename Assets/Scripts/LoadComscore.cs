using System;
using UnityEngine;

public class LoadComscore : MonoBehaviour
{
	private AndroidJavaObject objectComScore;

	private void Awake()
	{
		SetupComscore();
	}

	public void SetupComscore()
	{
		AndroidJavaClass androidJavaClass;
		try
		{
			androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("InitComscoreCommand.InitComscore: Failed to create classUnityPlayer: " + ex.Message);
			return;
			IL_002b:;
		}
		AndroidJavaObject @static;
		try
		{
			@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogWarning("InitComscoreCommand.InitComscore: Failed to get reference to activityUnityPlayer: " + ex2.Message);
			return;
			IL_005c:;
		}
		AndroidJavaObject androidJavaObject;
		try
		{
			androidJavaObject = new AndroidJavaObject("com.disney.mobilenetwork.utils.BuildSettings", @static);
			androidJavaObject.Call("LoadSettings");
		}
		catch (Exception ex3)
		{
			UnityEngine.Debug.LogWarning("InitComscoreCommand.InitComscore: Failed to get reference to buildSettings: " + ex3.Message);
			return;
			IL_00ab:;
		}
		AndroidJavaObject androidJavaObject2;
		try
		{
			androidJavaObject2 = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
		}
		catch (Exception ex4)
		{
			UnityEngine.Debug.LogWarning("InitComscoreCommand.InitComscore: Failed to get reference to objectApplication: " + ex4.Message);
			return;
			IL_00e5:;
		}
		try
		{
			objectComScore = new AndroidJavaObject("com.disney.mobilenetwork.plugins.ComScorePlugin");
		}
		catch (Exception ex5)
		{
			UnityEngine.Debug.LogWarning("InitComscoreCommand.InitComscore: Failed to get reference to classComScore: " + ex5.Message);
			return;
			IL_0122:;
		}
		try
		{
			objectComScore.Call("init", androidJavaObject2, androidJavaObject);
			UnityEngine.Debug.Log("Successfully executed init method of the ComScorePlugin");
		}
		catch (Exception ex6)
		{
			UnityEngine.Debug.LogWarning("Unable to call init on the Comscore class: " + ex6.Message);
			goto end_IL_0156;
			IL_0173:
			end_IL_0156:;
		}
	}

	private void OnApplicationPause()
	{
		try
		{
			objectComScore.Call("onPause");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("Unable to call onPause on the Comscore class: " + ex.Message);
			goto end_IL_001b;
			IL_0036:
			end_IL_001b:;
		}
	}

	private void OnApplicationFocus()
	{
		try
		{
			objectComScore.Call("onResume");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("Unable to call onResume on the Comscore class: " + ex.Message);
			goto end_IL_001b;
			IL_0036:
			end_IL_001b:;
		}
	}
}
