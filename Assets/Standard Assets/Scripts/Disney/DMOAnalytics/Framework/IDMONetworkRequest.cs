using System.Collections.Generic;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public interface IDMONetworkRequest
	{
		MonoBehaviour GameObj
		{
			set;
		}

		void StartCoroutine(Dictionary<string, object> GameData);

		void FlushQueue();
	}
}
