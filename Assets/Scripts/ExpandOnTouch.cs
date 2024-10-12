using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandOnTouch : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public bool ExpandWidth;

	public float WidthTargetSize = 100f;

	public bool ExpandHeight;

	public float HeightTargetSize = 200f;

	public float ShrinkTimeSec = 1f;

	private float originalWidth;

	private float originalHeight;

	private bool snapToOriginal;

	private RectTransform rectTransform;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		originalWidth = rectTransform.rect.width;
		originalHeight = rectTransform.rect.height;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (ExpandWidth)
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, WidthTargetSize);
		}
		if (ExpandHeight)
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, HeightTargetSize);
		}
		StopCoroutine("SnapToOriginalDimensions");
		snapToOriginal = false;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		snapToOriginal = true;
		StartCoroutine("SnapToOriginalDimensions");
	}

	private IEnumerator SnapToOriginalDimensions()
	{
		yield return new WaitForSeconds(ShrinkTimeSec);
		if (!snapToOriginal)
		{
			/*Error near IL_0054: Unexpected return in MoveNext()*/;
		}
		if (ExpandWidth)
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalWidth);
		}
		if (ExpandHeight)
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalHeight);
		}
	}
}
