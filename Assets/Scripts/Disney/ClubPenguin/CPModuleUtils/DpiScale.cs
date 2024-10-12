using UnityEngine;

namespace Disney.ClubPenguin.CPModuleUtils
{
	[RequireComponent(typeof(RectTransform))]
	public class DpiScale : MonoBehaviour
	{
		public enum ScaleOffsetDirection
		{
			None,
			Top,
			Bottom,
			Left,
			Right,
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		public float TargetDPI = 280f;

		public float ScreenInchesDiagLessThan = 6f;

		public ScaleOffsetDirection OffsetDirectionOnScale;

		private void Start()
		{
			if (Screen.dpi < TargetDPI || TargetDPI <= 0f)
			{
				return;
			}
			float num = (float)Screen.width / Screen.dpi;
			float num2 = (float)Screen.height / Screen.dpi;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 >= ScreenInchesDiagLessThan)
			{
				return;
			}
			RectTransform component = GetComponent<RectTransform>();
			float num4 = Screen.dpi / TargetDPI * ScreenInchesDiagLessThan / num3;
			UnityEngine.Debug.Log("=================== Detected hand held device. Scaling GameObject to: " + num4 + ".  Screen diag inches = " + num3 + ", dpi = " + Screen.dpi);
			float width = component.rect.width;
			float height = component.rect.height;
			component.sizeDelta = new Vector2(width * num4, height * num4);
			float num5 = width * num4 - width;
			float num6 = height * num4 - height;
			if (OffsetDirectionOnScale != 0)
			{
				float num7 = 0f;
				float num8 = 0f;
				switch (OffsetDirectionOnScale)
				{
				case ScaleOffsetDirection.Left:
					num7 = -1f;
					break;
				case ScaleOffsetDirection.Right:
					num7 = 1f;
					break;
				case ScaleOffsetDirection.Top:
					num8 = 1f;
					break;
				case ScaleOffsetDirection.Bottom:
					num8 = -1f;
					break;
				case ScaleOffsetDirection.BottomLeft:
					num7 = -1f;
					num8 = -1f;
					break;
				case ScaleOffsetDirection.BottomRight:
					num7 = 1f;
					num8 = -1f;
					break;
				case ScaleOffsetDirection.TopLeft:
					num7 = -1f;
					num8 = 1f;
					break;
				case ScaleOffsetDirection.TopRight:
					num7 = 1f;
					num8 = 1f;
					break;
				}
				component.Translate(new Vector3(num5 / 2f * num7, num6 / 2f * num8, 0f));
			}
		}
	}
}
