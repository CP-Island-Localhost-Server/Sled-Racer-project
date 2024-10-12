using Disney.DMOAnalytics;
using UnityEngine;

public class DMOAnalyticsAutoLog : MonoBehaviour
{
	public string DMOAnalyticsKey;

	public string DMOAnalyticsSecret;

	public string ApplicationBundleIdentifer;

	public string ApplicationVersion;

	private void Start()
	{
		DMOAnalytics.SharedAnalytics.LogAppStart();
	}

	private void Update()
	{
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		DMOAnalytics.SharedAnalytics.initWithAnalyticsKeySerect(this, DMOAnalyticsKey, DMOAnalyticsSecret);
	}

	private void OnApplicationPause(bool paused)
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

	private void OnApplicationQuit()
	{
		DMOAnalytics.SharedAnalytics.LogAppEnd();
	}
}
