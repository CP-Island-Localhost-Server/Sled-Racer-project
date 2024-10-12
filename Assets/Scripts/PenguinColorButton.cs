using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PenguinColorButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public delegate void ColorButtonDelegate(PenguinColorButton button);

	public Sprite penguinSprite;

	public Image highlightImage;

	public PenguinCreateColor penguinColorEnum;

	public ColorButtonDelegate OnColorButtonClick;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (OnColorButtonClick != null)
		{
			OnColorButtonClick(this);
		}
	}

	public void ShowHaloSelection()
	{
		highlightImage.enabled = true;
	}

	public void HideHaloSelection()
	{
		highlightImage.enabled = false;
	}
}
