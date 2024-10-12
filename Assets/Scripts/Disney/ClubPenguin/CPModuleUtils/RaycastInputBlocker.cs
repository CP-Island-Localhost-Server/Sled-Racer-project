using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.CPModuleUtils
{
	public class RaycastInputBlocker
	{
		private static List<GraphicRaycaster> ignoredRayCasters = new List<GraphicRaycaster>();

		private static List<GraphicRaycaster> blockedRayCasters = new List<GraphicRaycaster>();

		private static List<bool> blockedCastersPrevStates = new List<bool>();

		public static void AllowGraphicRayCaster(GraphicRaycaster rayCaster)
		{
			if (!ignoredRayCasters.Contains(rayCaster))
			{
				ignoredRayCasters.Add(rayCaster);
				int num = blockedRayCasters.IndexOf(rayCaster);
				if (num > -1)
				{
					blockedRayCasters.RemoveAt(num);
					blockedCastersPrevStates.RemoveAt(num);
				}
			}
		}

		public static void BlockRayCasterInput(GraphicRaycaster rayCaster)
		{
			if (!blockedRayCasters.Contains(rayCaster) && !ignoredRayCasters.Contains(rayCaster))
			{
				blockedRayCasters.Add(rayCaster);
				blockedCastersPrevStates.Add(rayCaster.enabled);
				rayCaster.enabled = false;
			}
		}

		public static void BlockRayCasterInputsArray(GraphicRaycaster[] rayCasters)
		{
			for (int i = 0; i < rayCasters.Length; i++)
			{
				BlockRayCasterInput(rayCasters[i]);
			}
		}

		public static void RestoreAllRayCastersInput()
		{
			for (int i = 0; i < blockedRayCasters.Count; i++)
			{
				blockedRayCasters[i].enabled = blockedCastersPrevStates[i];
			}
			blockedRayCasters.Clear();
			blockedCastersPrevStates.Clear();
		}

		public static void Clear()
		{
			RestoreAllRayCastersInput();
			ignoredRayCasters.Clear();
		}

		public static GraphicRaycaster GetFirstRayCasterInHierarchy(RectTransform rectTransform)
		{
			RectTransform rectTransform2 = rectTransform;
			while (rectTransform2 != null)
			{
				if (rectTransform2.GetComponent<GraphicRaycaster>() != null)
				{
					return rectTransform2.GetComponent<GraphicRaycaster>();
				}
				rectTransform2 = (rectTransform2.parent as RectTransform);
			}
			return null;
		}
	}
}
