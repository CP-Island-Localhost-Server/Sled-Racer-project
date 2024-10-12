using UnityEngine;

namespace Disney.ClubPenguin.Common.Utils
{
	public class PhysicsUtil
	{
		public static bool AttachObjectToLayer(GameObject obj, LayerMask mask, float maxDistance, Vector3 raycastDirection, Vector3? offset = default(Vector3?))
		{
			Transform transform = obj.transform;
			raycastDirection.Normalize();
			Vector3 origin = transform.position + -raycastDirection * maxDistance;
			if (Physics.Raycast(origin, raycastDirection, maxDistance, mask))
			{
				return false;
			}
			RaycastHit[] array = Physics.RaycastAll(transform.position, raycastDirection, maxDistance, mask);
			RaycastHit raycastHit = default(RaycastHit);
			raycastHit.distance = maxDistance + 1f;
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit2 = array[i];
				if (raycastHit2.distance < raycastHit.distance)
				{
					raycastHit = raycastHit2;
				}
			}
			if (raycastHit.distance < maxDistance && raycastHit.distance != 0f)
			{
				transform.position += raycastDirection * raycastHit.distance;
				if (offset.HasValue)
				{
					transform.position += offset.Value;
				}
				return true;
			}
			return false;
		}
	}
}