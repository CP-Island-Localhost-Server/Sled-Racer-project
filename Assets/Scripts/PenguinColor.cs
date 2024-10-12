using System;
using UnityEngine;
using UnityEngine.UI;

public class PenguinColor : MonoBehaviour
{
	public GameObject buttonContainerObject;

	public Image penguinImageReference;

	public PenguinColorButton defaultColor;

	public AudioClip updateColorAudioClip;

	[HideInInspector]
	public AudioSource RootAudioSource;

	[HideInInspector]
	public PenguinCreateColor Color;

	private PenguinColorButton[] penguinColorButtonList;

	private PenguinColorButton currentColorSelection;

	private void Start()
	{
		penguinColorButtonList = buttonContainerObject.GetComponentsInChildren<PenguinColorButton>();
		for (int i = 0; i < penguinColorButtonList.Length; i++)
		{
			PenguinColorButton obj = penguinColorButtonList[i];
			obj.OnColorButtonClick = (PenguinColorButton.ColorButtonDelegate)Delegate.Combine(obj.OnColorButtonClick, new PenguinColorButton.ColorButtonDelegate(OnColorButtonClickHandler));
		}
		int num = UnityEngine.Random.Range(0, penguinColorButtonList.Length - 1);
		currentColorSelection = penguinColorButtonList[num];
		SetPenguinColor(currentColorSelection);
	}

	private void OnDestroy()
	{
		for (int i = 0; i < penguinColorButtonList.Length; i++)
		{
			PenguinColorButton obj = penguinColorButtonList[i];
			obj.OnColorButtonClick = (PenguinColorButton.ColorButtonDelegate)Delegate.Remove(obj.OnColorButtonClick, new PenguinColorButton.ColorButtonDelegate(OnColorButtonClickHandler));
		}
	}

	public void OnColorButtonClickHandler(PenguinColorButton penguinColorButton)
	{
		RootAudioSource.PlayOneShot(updateColorAudioClip);
		if (currentColorSelection != penguinColorButton)
		{
			currentColorSelection = penguinColorButton;
			SetPenguinColor(penguinColorButton);
		}
	}

	private void SetPenguinColor(PenguinColorButton penguinColorButton)
	{
		HideHaloSelections();
		penguinColorButton.ShowHaloSelection();
		penguinImageReference.overrideSprite = penguinColorButton.penguinSprite;
		Color = penguinColorButton.penguinColorEnum;
	}

	private void HideHaloSelections()
	{
		for (int i = 0; i < penguinColorButtonList.Length; i++)
		{
			penguinColorButtonList[i].HideHaloSelection();
		}
	}
}
