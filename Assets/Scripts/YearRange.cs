using System;
using UnityEngine;
using UnityEngine.UI;

public class YearRange : MonoBehaviour
{
	public RectTransform Content;

	public Text TextElement;

	public int YearInPast = 120;

	private void Start()
	{
		int year = DateTime.Now.Year;
		int num = year - YearInPast;
		for (int num2 = year; num2 >= num; num2--)
		{
			Text text = UnityEngine.Object.Instantiate(TextElement) as Text;
			string arg = (num2 >= 10) ? string.Empty : "0";
			text.text = arg + num2;
			text.GetComponent<RectTransform>().SetParent(Content, worldPositionStays: false);
		}
	}
}
