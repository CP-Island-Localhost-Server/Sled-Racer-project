using UnityEngine;

namespace Disney.ClubPenguin.Common.Utils
{
	public static class LayerMaskUtil
	{
		public static void Set(ref LayerMask mask, int layer)
		{
			mask = ((int)mask | (1 << (layer & 0x1F)));
		}

		public static void Clear(ref LayerMask mask, int layer)
		{
			mask = ((int)mask & ~(1 << (layer & 0x1F)));
		}

		public static void ClearAll(ref LayerMask mask)
		{
			mask = 0;
		}

		public static void SetAll(ref LayerMask mask)
		{
			mask = -1;
		}
	}
}
