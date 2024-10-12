using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public static class AvatarUtil
	{
		public static int[] COLOR_TO_RESOURCE_MAP = new int[17]
		{
			1,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			1,
			14,
			15
		};

		public static int GetResourceId(int colorId)
		{
			int result = 1;
			if (colorId >= 0 && colorId < COLOR_TO_RESOURCE_MAP.Length)
			{
				result = COLOR_TO_RESOURCE_MAP[colorId];
			}
			return result;
		}

		public static Sprite GetLargeAvatar(int colorId)
		{
			return Resources.Load<Sprite>("AvatarSprites/Pengiun_" + GetResourceId(colorId));
		}

		public static Sprite GetSmallAvatar(int colorId)
		{
			return Resources.Load<Sprite>("AvatarSpritesSmall/Pengiun_" + GetResourceId(colorId));
		}

		public static Material GetRiderMaterial(int colorId)
		{
			return Resources.Load<Material>("RiderMaterials/RiderMaterial_" + GetResourceId(colorId));
		}
	}
}
