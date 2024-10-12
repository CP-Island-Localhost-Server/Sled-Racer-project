using Disney.ClubPenguin.CPModuleUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.WebPageViewer
{
	public class DisplayNativeWebPage : MonoBehaviour
	{
		public delegate void ClosedDelegate();

		public ClosedDelegate Closed;

		public RectTransform container;

		public Button CloseButton;

		private NativeWebView nativeWebView;

		private void Awake()
		{
			nativeWebView = new NativeWebView();
		}

		private void Start()
		{
			if (CloseButton != null)
			{
				HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton, visible: false);
			}
		}

		private void OnDestroy()
		{
			if (nativeWebView != null)
			{
				nativeWebView.Close();
				nativeWebView = null;
			}
		}

		public void OnCloseButton()
		{
			if (nativeWebView != null)
			{
				nativeWebView.Close();
				nativeWebView = null;
			}
			if (Closed != null)
			{
				Closed();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void DisplayPage(string pageURL)
		{
			if (!(pageURL == string.Empty))
			{
				Rect rectInPhysicalScreenSpace = GetRectInPhysicalScreenSpace(container);
				nativeWebView.Show(pageURL, rectInPhysicalScreenSpace);
			}
		}

		private static Rect GetRectInPhysicalScreenSpace(RectTransform aRectTransform)
		{
			Vector3[] array = new Vector3[4];
			aRectTransform.GetWorldCorners(array);
			Canvas parentCanvas = GetParentCanvas(aRectTransform.gameObject);
			if (parentCanvas == null)
			{
				return new Rect(0f, 0f, 0f, 0f);
			}
			Camera worldCamera = parentCanvas.worldCamera;
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(worldCamera, array[0]);
			Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(worldCamera, array[1]);
			Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(worldCamera, array[2]);
			float num = (float)Display.displays[0].systemWidth / (float)Screen.width;
			float num2 = (float)Display.displays[0].systemHeight / (float)Screen.height;
			int num3 = Mathf.CeilToInt((vector3.x - vector2.x) * num);
			int num4 = Mathf.CeilToInt((vector2.y - vector.y) * num2);
			int num5 = (int)(vector2.x * num);
			int num6 = (int)(vector2.y * num2);
			num6 = Display.displays[0].systemHeight - num6;
			int num7 = (int)GetAndroidDPIDensity();
			if (num7 != 0)
			{
				num3 /= num7;
				num4 /= num7;
				num5 /= num7;
				num6 /= num7;
			}
			return new Rect(num5, num6, num3, num4);
		}

		private static Canvas GetParentCanvas(GameObject aGameObject)
		{
			Canvas canvas = null;
			Transform parent = aGameObject.transform.parent;
			while (canvas == null && parent != null)
			{
				canvas = parent.GetComponent<Canvas>();
				parent = parent.parent;
			}
			return canvas;
		}

		private static float GetAndroidDPIDensity()
		{
			return Screen.dpi;
		}
	}
}
