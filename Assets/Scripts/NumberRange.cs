using UnityEngine;
using UnityEngine.UI;

public class NumberRange : MonoBehaviour
{
	public RectTransform Content;

	public int start;

	public int end;

	public Text TextElement;

	private void Start()
	{
		for (int i = start; i <= end; i++)
		{
			Text text = UnityEngine.Object.Instantiate(TextElement) as Text;
			string arg = (i >= 10) ? string.Empty : "0";
			text.text = arg + i;
			text.GetComponent<RectTransform>().SetParent(Content, worldPositionStays: false);
		}
	}
}
