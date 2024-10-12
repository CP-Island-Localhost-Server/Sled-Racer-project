using Disney.DMOAnalytics;
using UnityEngine;

public class DMOAnalyticsSetup : MonoBehaviour
{
	public string AppleDMOAnalyticsKey;

	public string AppleDMOAnalyticsSecret;

	public string GoogleDMOAnalyticsKey;

	public string GoogleDMOAnalyticsSecret;

	public string ApplicationBundleIdentifer;

	public string ApplicationVersion;

	public bool Enabled = true;

	private void Awake()
	{
		if (Enabled)
		{
			DMOAnalytics.SharedAnalytics.initWithAnalyticsKeySerect(this, GoogleDMOAnalyticsKey, GoogleDMOAnalyticsSecret);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void Start()
	{
		SetupAnalytics();
	}

	private void SetupAnalytics()
	{
		if (Enabled)
		{
			DMOAnalytics.SharedAnalytics.LogAppStart();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (Enabled)
		{
			if (paused)
			{
				DMOAnalytics.SharedAnalytics.LogAppBackground();
			}
			else
			{
				DMOAnalytics.SharedAnalytics.LogAppForeground();
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (Enabled)
		{
			DMOAnalytics.SharedAnalytics.LogAppEnd();
		}
	}
}
