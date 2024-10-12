using System;
using UnityEngine;

public class HeadphoneUnplugDetection : MonoBehaviour
{
	public static event Action OnHeadphoneUnplugged;

	private void Start()
	{
	}

	private void headPhoneUnplugged(string message)
	{
		if (HeadphoneUnplugDetection.OnHeadphoneUnplugged != null)
		{
			HeadphoneUnplugDetection.OnHeadphoneUnplugged();
		}
	}
}
