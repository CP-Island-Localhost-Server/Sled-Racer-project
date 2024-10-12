using UnityEngine;

public class DestroyMeFromEvent : MonoBehaviour
{
	private void DestroyMe()
	{
		UnityEngine.Debug.Log("Heard a DestroyMe call");
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
