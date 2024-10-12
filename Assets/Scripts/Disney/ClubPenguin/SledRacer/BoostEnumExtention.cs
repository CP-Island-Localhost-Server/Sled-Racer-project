using System;
using System.Reflection;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public static class BoostEnumExtention
	{
		public static string GetTitle(this BoostManager.AvailableBoosts b)
		{
			BoostManager.BoostAttribute attr = GetAttr(b);
			return attr.Title;
		}

		public static string GetDescription(this BoostManager.AvailableBoosts b)
		{
			BoostManager.BoostAttribute attr = GetAttr(b);
			return attr.Description;
		}

		public static string GetImageName(this BoostManager.AvailableBoosts b)
		{
			BoostManager.BoostAttribute attr = GetAttr(b);
			return attr.ImageName;
		}

		public static Type GetImplementationClass(this BoostManager.AvailableBoosts b)
		{
			BoostManager.BoostAttribute attr = GetAttr(b);
			return attr.ImplementationClass;
		}

		public static Sprite GetSprite(this BoostManager.AvailableBoosts b)
		{
			return Resources.Load<Sprite>("BoostSprites/" + b.GetImageName());
		}

		public static Sprite GetHudSprite(this BoostManager.AvailableBoosts b)
		{
			return Resources.Load<Sprite>("BoostSprites/Hud/" + b.GetImageName());
		}

		private static BoostManager.BoostAttribute GetAttr(BoostManager.AvailableBoosts b)
		{
			return (BoostManager.BoostAttribute)Attribute.GetCustomAttribute(ForValue(b), typeof(BoostManager.BoostAttribute));
		}

		private static MemberInfo ForValue(BoostManager.AvailableBoosts b)
		{
			return typeof(BoostManager.AvailableBoosts).GetField(Enum.GetName(typeof(BoostManager.AvailableBoosts), b));
		}
	}
}
