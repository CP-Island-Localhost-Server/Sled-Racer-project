using UnityEngine;

public class SwapUIElementVisablity : MonoBehaviour
{
	public GameObject OtherObject;

	public float SwapSeconds = 3f;

	private void Start()
	{
		Invoke("Swap", SwapSeconds);
	}

	private void Swap()
	{
		OtherObject.SetActive(base.gameObject.activeSelf);
		base.gameObject.SetActive(!base.gameObject.activeSelf);
		Invoke("Swap", SwapSeconds);
	}
}
