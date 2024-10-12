using UnityEngine;

namespace Disney.ExtendedTestTools.Utils
{
	public static class Utils
	{
		public static GameObject FindChildObject(GameObject parentObject, string childName)
		{
			Transform component = parentObject.GetComponent<Transform>();
			for (int i = 0; i < component.childCount; i++)
			{
				Transform child = component.GetChild(i);
				if (child.name == childName)
				{
					return child.gameObject;
				}
				GameObject gameObject = FindChildObject(child.gameObject, childName);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return null;
		}
	}
}
