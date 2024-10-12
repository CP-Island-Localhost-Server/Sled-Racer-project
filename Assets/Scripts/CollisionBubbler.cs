using UnityEngine;

public class CollisionBubbler : MonoBehaviour
{
	private GameObject parent;

	public bool PropegateToAllAncenstors;

	private void Start()
	{
		Transform transform = GetComponent<Transform>().parent;
		if (transform != null)
		{
			parent = transform.gameObject;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		PropegateCollision("OnCollisionEnter", other);
	}

	private void OnTriggerEnter(Collider other)
	{
		PropegateCollision("OnTriggerEnter", other);
	}

	private void PropegateCollision(string message, object other)
	{
		if (parent != null)
		{
			if (PropegateToAllAncenstors)
			{
				SendMessageUpwards(message, other, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				parent.SendMessage(message, other);
			}
		}
	}
}
