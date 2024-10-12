using UnityEngine;

namespace Disney.ClubPenguin.CPModuleUtils
{
	public static class GameObjectExtensions
	{
		public static string GetPath(this Transform current)
		{
			if (current.parent == null)
			{
				return "/" + current.name;
			}
			return current.parent.GetPath() + "/" + current.name;
		}

		public static string GetPath(this GameObject go)
		{
			if (go.transform == null)
			{
				return string.Empty;
			}
			return go.transform.GetPath();
		}

		public static string GetPath(this Component component)
		{
			if (component.transform == null)
			{
				return string.Empty;
			}
			return component.transform.GetPath() + "/" + component.GetType().ToString();
		}
	}
}
