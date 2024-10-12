using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.CPModuleUtils
{
	public static class GameObjectUtil
	{
		public static void CleanupImageReferences(GameObject go)
		{
			Image[] componentsInChildren = go.GetComponentsInChildren<Image>();
			Image[] array = componentsInChildren;
			foreach (Image image in array)
			{
				image.sprite = null;
			}
		}
	}
}
